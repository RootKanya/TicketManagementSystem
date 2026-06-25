using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ticketing.Application.Commands;
using Ticketing.Domain.Repositories;

namespace Ticketing.Application.CommandHandlers
{
    public class CheckInTicketHandler : IRequestHandler<CheckInTicketCommand, bool>
    {
        private readonly ITicketRepository _ticketRepository;

        public CheckInTicketHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<bool> Handle(CheckInTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByCodeAsync(request.TicketCode);
            if (ticket == null)
                throw new Exception("Invalid ticket: Ticket code not found.");

            if (ticket.EventId != request.EventId)
                throw new Exception("Invalid ticket: Ticket does not match the current event.");

            ticket.CheckIn();

            await _ticketRepository.UpdateAsync(ticket);

            return true;
        }
    }
}