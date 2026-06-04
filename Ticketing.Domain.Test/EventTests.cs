using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Tests;

public class EventTests
{
    [Fact]
    public void Event_Should_ThrowException_When_EndDateIsBeforeStartDate()
    {
        var startDate = DateTime.UtcNow.AddDays(2);
        var endDate = DateTime.UtcNow.AddDays(1);

        var exception = Assert.Throws<ArgumentException>(() =>
            new Event(Guid.NewGuid(), "ITS Music Fest", 100, startDate, endDate));

        Assert.Equal("End date cannot be earlier than start date.", exception.Message);
    }

    [Fact]
    public void Event_Should_ThrowException_When_CapacityIsZeroOrNegative()
    {
        var validStart = DateTime.UtcNow.AddDays(1);
        var validEnd = DateTime.UtcNow.AddDays(2);

        var exception = Assert.Throws<ArgumentException>(() =>
            new Event(Guid.NewGuid(), "Tech Seminar", 0, validStart, validEnd));

        Assert.Equal("Maximum capacity must be greater than zero.", exception.Message);
    }

    [Fact]
    public void Publish_Should_ThrowException_When_NoActiveTicketCategories()
    {
        var @event = new Event(Guid.NewGuid(), "Tech Talk", 50, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2));

        var exception = Assert.Throws<InvalidOperationException>(() => @event.Publish());

        Assert.Equal("Cannot publish an event without active ticket categories.", exception.Message);
    }

    [Fact]
    public void AddTicketCategory_Should_ThrowException_When_QuotaExceedsCapacity()
    {
        var @event = new Event(Guid.NewGuid(), "Workshop", 100, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2));
        var price = Money.Create(50000);

        @event.AddTicketCategory("Regular", price, 80);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            @event.AddTicketCategory("VIP", price, 30));

        Assert.Equal("Total ticket quota exceeds event maximum capacity.", exception.Message);
    }
}