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

public class BookingTests
{
    [Fact]
    public void CreateBooking_ForUnpublishedEvent_ThrowsInvalidOperationException()
    {
        // Arrange
        var draftEvent = new Event(
            Guid.NewGuid(), "Draft Event", "Desc",
            new EventSchedule(DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddDays(11)),
            "Loc", new EventCapacity(100));

        var categoryId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            new Booking(Guid.NewGuid(), Guid.NewGuid(), draftEvent, categoryId, new TicketQuantity(2), 5000m)
        );

        Assert.Equal("A booking can only be created for an event with the status Published.", exception.Message);
    }
}