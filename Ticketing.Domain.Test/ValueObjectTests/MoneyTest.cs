using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Test.ValueObjectTests;


public class MoneyTest
{
    [Theory]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public void Constructor_WithNegativeAmount_ThrowsArgumentException(decimal invalidAmount)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Money(invalidAmount, "IDR"));
        Assert.Equal("Total price cannot be negative.", exception.Message);
    }

    [Fact]
    public void Constructor_WithValidAmount_CreatesInstance()
    {
        // Act
        var money = new Money(150000, "IDR");

        // Assert
        Assert.Equal(150000, money.Amount);
        Assert.Equal("IDR", money.Currency);
    }
}