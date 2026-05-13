using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Tests;

public class BookingTests
{
    [Fact]
    public void CreateBooking_Should_ThrowException_When_QuantityExceedsQuota()
    {
        var categoryId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var requestedQty = 5;
        var remainingQuota = 2; 

        // "IF RequestedQty > RemainingQuota THEN Reject(Fault): 'Requested quantity exceeds remaining ticket quota.'"
        var exception = Assert.Throws<InvalidOperationException>(() =>
            Booking.Create(Guid.NewGuid(), customerId, categoryId, requestedQty, remainingQuota));

        Assert.Equal("Requested quantity exceeds remaining ticket quota.", exception.Message);
    }

    [Fact]
    public void CreateBooking_Should_SetStatusToPending_And_RaiseTicketReserved()
    {
        var categoryId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var requestedQty = 2;
        var remainingQuota = 100;

        // "Create Booking AND Set Status = PendingPayment AND Start 15-Min Timer AND Raise TicketReserved."
        var booking = Booking.Create(Guid.NewGuid(), customerId, categoryId, requestedQty, remainingQuota);
        Assert.Equal(BookingStatus.PendingPayment, booking.Status);

        var expectedExpiry = DateTime.UtcNow.AddMinutes(15);
        Assert.True((expectedExpiry - booking.PaymentDeadline).TotalSeconds < 5); 

        Assert.Contains(booking.DomainEvents, e => e is TicketReserved);
    }
}
