using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Tests;

public class RefundTests
{
    [Fact]
    public void RequestRefund_Should_ThrowException_When_TicketIsCheckedIn()
    {
        var bookingId = Guid.NewGuid();
        var hasCheckedInTickets = true;

        var exception = Assert.Throws<InvalidOperationException>(() =>
            Refund.Request(Guid.NewGuid(), bookingId, hasCheckedInTickets, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)));

        Assert.Equal("Cannot refund a booking if tickets have been used.", exception.Message);
    }

    [Fact]
    public void Approve_Should_ThrowException_When_StatusIsNotRequested()
    {
        var refund = Refund.Request(Guid.NewGuid(), Guid.NewGuid(), false, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        refund.Reject("Invalid request");

        var exception = Assert.Throws<InvalidOperationException>(() =>
            refund.Approve());

        Assert.Equal("Refund can only be approved if its status is Requested.", exception.Message);
    }

    [Fact]
    public void Reject_Should_ThrowException_When_ReasonIsEmpty()
    {
        var refund = Refund.Request(Guid.NewGuid(), Guid.NewGuid(), false, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

        var exception = Assert.Throws<ArgumentException>(() =>
            refund.Reject(""));

        Assert.Equal("A rejection reason is required.", exception.Message);
    }
}