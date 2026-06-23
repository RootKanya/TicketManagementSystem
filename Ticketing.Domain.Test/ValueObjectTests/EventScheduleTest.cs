using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Test.ValueObjectTests;

public class EventScheduleTests
{
    [Fact]
    public void Constructor_EndDateBeforeStartDate_ThrowsArgumentException()
    {
        // Arrange
        var startDate = new DateTime(2026, 1, 10);
        var endDate = new DateTime(2026, 1, 9); // End date lebih awal

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new EventSchedule(startDate, endDate));
        Assert.Equal("The event cannot be created if the end date is earlier than the start date.", exception.Message);
    }
}