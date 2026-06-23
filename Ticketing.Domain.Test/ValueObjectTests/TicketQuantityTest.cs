using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Test.ValueObjectTests;

public class TicketQuantityTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Constructor_ShouldThrowArgumentException_WhenQuantityIsZeroOrNegative(int invalidQuantity)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new TicketQuantity(invalidQuantity));
        Assert.Equal("Ticket quantity/quota must be greater than zero.", exception.Message);
    }
}
