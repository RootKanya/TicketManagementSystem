using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.CommandHandlers;

using global::Ticketing.Application.Commands;
using global::Ticketing.Domain.Repositories;
using global::Ticketing.Domain.Services;
using global::Ticketing.Domain.ValueObjects;

public class CreateTicketBookingCommandHandler
{
    private readonly IEventRepository _eventRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly BookingDomainService _bookingDomainService;

    public CreateTicketBookingCommandHandler(
        IEventRepository eventRepository, IBookingRepository bookingRepository, BookingDomainService bookingDomainService)
    {
        _eventRepository = eventRepository;
        _bookingRepository = bookingRepository;
        _bookingDomainService = bookingDomainService;
    }

    public async Task<Guid> Handle(CreateTicketBookingCommand command, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(command.EventId, cancellationToken)
            ?? throw new KeyNotFoundException("Event not found.");

        var booking = await _bookingDomainService.CreateBookingAsync(
            command.CustomerId, @event, command.CategoryId,
            new TicketQuantity(command.Quantity), 5000m, cancellationToken);

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _eventRepository.UpdateAsync(@event, cancellationToken);

        return booking.Id;
    }
}
