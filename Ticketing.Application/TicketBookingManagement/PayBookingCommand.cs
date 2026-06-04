using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Ticketing.Application.Shared;
using Ticketing.Domain.Aggregates.TicketAggregate;
using Ticketing.Domain.Repositories;

namespace Ticketing.Application.TicketBookingManagement;

public record PayBookingCommand(Guid BookingId, decimal Amount) : IRequest;

public class PayBookingCommandHandler : IRequestHandler<PayBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IPaymentService _paymentService;

    public PayBookingCommandHandler(IBookingRepository bookingRepository, ITicketRepository ticketRepository, IPaymentService paymentService)
    {
        _bookingRepository = bookingRepository;
        _ticketRepository = ticketRepository;
        _paymentService = paymentService;
    }

    public async Task Handle(PayBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken)
            ?? throw new KeyNotFoundException("Booking record missing.");

        booking.Pay(request.Amount);

        bool paymentApproved = await _paymentService.ProcessPaymentAsync(booking.Id, request.Amount, cancellationToken);
        if (!paymentApproved)
            throw new InvalidOperationException("Payment rejected by third-party financial provider gateway.");

        var issuedTickets = new List<Ticket>();
        for (int i = 0; i < booking.Quantity; i++)
        {
            string cleanCode = $"TCK-{booking.Id.ToString()[..8].ToUpper()}-{Guid.NewGuid().ToString()[..4].ToUpper()}";
            issuedTickets.Add(Ticket.Issue(booking.Id, booking.EventId, cleanCode));
        }

        await _ticketRepository.AddRangeAsync(issuedTickets, cancellationToken);
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
    }
}