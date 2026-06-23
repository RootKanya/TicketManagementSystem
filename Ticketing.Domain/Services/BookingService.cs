using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Repositories;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Services;

public class BookingDomainService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingDomainService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Booking> CreateBookingAsync(
        Guid customerId,
        Event @event,
        Guid categoryId,
        TicketQuantity quantity,
        decimal serviceFee,
        CancellationToken cancellationToken = default)
    {
        bool hasActiveBooking = await _bookingRepository.HasActiveBookingForEventAsync(customerId, @event.Id, cancellationToken);

        if (hasActiveBooking)
        {
            throw new InvalidOperationException("A customer cannot have more than one active booking for the same event.");
        }

        var booking = new Booking(
            Guid.NewGuid(),
            customerId,
            @event,
            categoryId,
            quantity,
            serviceFee
        );

        return booking;
    }
}