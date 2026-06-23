using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Tests;

using Ticketing.Domain.Aggregates;
using Ticketing.Domain.ValueObjects;
using Ticketing.Domain.Enums;

public class EventTests
{
    private Event CreateValidDraftEvent(int capacity = 100)
    {
        return new Event(
            Guid.NewGuid(),
            "Tech Conference 2026",
            "Annual tech conf",
            new EventSchedule(DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddDays(12)),
            "Surabaya",
            new EventCapacity(capacity)
        );
    }

    [Fact]
    public void PublishEvent_WithoutActiveTicketCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var @event = CreateValidDraftEvent();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => @event.Publish());
        Assert.Equal("An event can only be published if it has at least one active ticket category.", exception.Message);
    }

    [Fact]
    public void AddTicketCategory_QuotaExceedsEventCapacity_ThrowsArgumentException()
    {
        // Arrange
        var @event = CreateValidDraftEvent(capacity: 100);

        var price = new Money(50000, "IDR");
        var salesPeriod = new SalesPeriod(DateTime.UtcNow, DateTime.UtcNow.AddDays(5));

        @event.AddTicketCategory("Regular", price, new TicketQuantity(60), salesPeriod);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            @event.AddTicketCategory("VIP", price, new TicketQuantity(50), salesPeriod));

        Assert.Equal("The total quota of all ticket categories must not exceed the maximum event capacity.", exception.Message);
    }

    [Fact]
    public void PublishEvent_WithValidData_ChangesStatusAndRaisesEvent()
    {
        // Arrange
        var @event = CreateValidDraftEvent(capacity: 100);
        @event.AddTicketCategory(
            "Regular",
            new Money(50000, "IDR"),
            new TicketQuantity(50),
            new SalesPeriod(DateTime.UtcNow, DateTime.UtcNow.AddDays(5))
        );

        @event.ClearEvents();

        // Act
        @event.Publish();

        // Assert
        Assert.Equal(EventStatus.Published, @event.Status);

        var domainEvent = @event.DomainEvents.FirstOrDefault();
        Assert.NotNull(domainEvent);
        Assert.Contains("EventPublished", domainEvent.GetType().Name);
    }
}