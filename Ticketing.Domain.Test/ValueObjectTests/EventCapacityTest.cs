using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Test.ValueObjectTests;

public class EventCapacityTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Constructor_ZeroOrNegativeCapacity_ThrowsArgumentException(int invalidCapacity)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new EventCapacity(invalidCapacity));
        Assert.Equal("The event cannot be created if the maximum capacity is less than or equal to zero.", exception.Message);
    }
}
