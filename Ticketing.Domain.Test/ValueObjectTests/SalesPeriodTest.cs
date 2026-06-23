using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Test.ValueObjectTests;

public class SalesPeriodTests
{
    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenEndDateIsEarlierThanStartDate()
    {
        // Arrange
        var startDate = DateTime.UtcNow.AddDays(2);
        var endDate = DateTime.UtcNow.AddDays(1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new SalesPeriod(startDate, endDate));
        Assert.Equal("Sales end date cannot be earlier than sales start date.", exception.Message);
    }

    [Fact]
    public void IsValidAgainstEventStartDate_ShouldReturnFalse_WhenSalesEndIsAfterEventStart()
    {
        // Arrange
        var salesPeriod = new SalesPeriod(DateTime.UtcNow, DateTime.UtcNow.AddDays(5));
        var eventStartDate = DateTime.UtcNow.AddDays(3); 

        // Act
        var result = salesPeriod.IsValidAgainstEventStartDate(eventStartDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAgainstEventStartDate_ShouldReturnTrue_WhenSalesEndIsBeforeEventStart()
    {
        // Arrange
        var salesPeriod = new SalesPeriod(DateTime.UtcNow, DateTime.UtcNow.AddDays(2));
        var eventStartDate = DateTime.UtcNow.AddDays(3);

        // Act
        var result = salesPeriod.IsValidAgainstEventStartDate(eventStartDate);

        // Assert
        Assert.True(result);
    }
}
