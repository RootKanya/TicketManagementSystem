using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Tests;

public class BookingTests
{
    [Fact]
    public void CreateBooking_Should_ThrowException_When_QuantityIsZero()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            Booking.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0, 100));

        Assert.Equal("Ticket quantity must be greater than zero.", exception.Message);
    }

    [Fact]
    public void Pay_Should_ThrowException_When_PaymentDeadlineHasPassed()
    {
        var booking = Booking.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, 100);
        var paymentAmount = Money.Create(50000);
        var timeOfPayment = DateTime.UtcNow.AddMinutes(20);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            booking.Pay(paymentAmount, timeOfPayment));

        Assert.Equal("Payment deadline has expired.", exception.Message);
    }

    [Fact]
    public void Pay_Should_ThrowException_When_AmountIsIncorrect()
    {
        var booking = Booking.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 2, 100);
        var incorrectPayment = Money.Create(80000);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            booking.Pay(incorrectPayment, DateTime.UtcNow));

        Assert.Equal("Payment amount does not match total price.", exception.Message);
    }

    [Fact]
    public void Expire_Should_ThrowException_When_BookingIsAlreadyPaid()
    {
        var booking = Booking.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, 100);
        booking.SetStatusForTesting(BookingStatus.Paid);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            booking.Expire());

        Assert.Equal("Booking is already paid and cannot be expired.", exception.Message);
    }
}