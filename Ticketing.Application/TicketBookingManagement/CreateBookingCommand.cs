using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Ticketing.Domain.Aggregates.BookingAggregate;
using Ticketing.Domain.Repositories;

namespace Ticketing.Application.TicketBookingManagement;

public record CreateBookingCommand(Guid CustomerId, Guid EventId, Guid TicketCategoryId, int Quantity) : IRequest<Guid>;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IEventRepository _eventRepository;

    public CreateBookingCommandHandler(IBookingRepository bookingRepository, IEventRepository eventRepository)
    {
        _bookingRepository = bookingRepository;
        _eventRepository = eventRepository;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new KeyNotFoundException("Event not found.");

        bool hasActive = await _bookingRepository.HasActiveBookingAsync(request.CustomerId, request.EventId, cancellationToken);
        if (hasActive)
            throw new InvalidOperationException("Customer already has an active booking for this event.");

        decimal baseServiceFee = 5000; 
        var booking = Booking.Create(request.CustomerId, @event, request.TicketCategoryId, request.Quantity, baseServiceFee);

        await _bookingRepository.AddAsync(booking, cancellationToken);
        return booking.Id;
    }
}
