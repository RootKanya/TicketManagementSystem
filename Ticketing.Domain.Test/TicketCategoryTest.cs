using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Tests;

public class TicketCategoryTests
{
    [Fact]
    public void CreateCategory_Should_ThrowException_When_PriceIsNegative()
    {
        // "IF Price < 0 THEN Reject(Fault): 'Ticket price cannot be negative.'"
        var exception = Assert.Throws<ArgumentException>(() =>
            Money.Create(-10000));

        Assert.Equal("Price cannot be negative.", exception.Message);
    }

    [Fact]
    public void CreateCategory_Should_ThrowException_When_QuotaIsZeroOrLess()
    {
        var price = Money.Create(50000);
        var eventId = Guid.NewGuid();

        // "IF Quota <= 0 THEN Reject(Fault): 'Ticket quota must be greater than zero.'"
        var exception = Assert.Throws<ArgumentException>(() =>
            new TicketCategory(Guid.NewGuid(), eventId, "VIP", price, 0, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)));

        Assert.Equal("Ticket quota must be greater than zero.", exception.Message);
    }
}