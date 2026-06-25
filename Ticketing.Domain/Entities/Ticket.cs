using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Enums;

namespace Ticketing.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; private set; }
        public Guid BookingId { get; private set; }
        public Guid EventId { get; private set; }
        public string TicketCode { get; private set; }
        public TicketStatus Status { get; private set; }

        private Ticket() { }

        public Ticket(Guid bookingId, Guid eventId, string ticketCode)
        {
            Id = Guid.NewGuid();
            BookingId = bookingId;
            EventId = eventId;
            TicketCode = ticketCode; 
            Status = TicketStatus.Active;
        }

        public void CheckIn()
        {
            if (Status == TicketStatus.CheckedIn)
                throw new InvalidOperationException("Ticket has already been used.");

            if (Status != TicketStatus.Active)
                throw new InvalidOperationException($"Cannot check in ticket. Current status is {Status}.");

            Status = TicketStatus.CheckedIn;

        }

        public void Cancel()
        {
            Status = TicketStatus.Cancelled;
        }

        public void MarkAsRefundRequired()
        {
            Status = TicketStatus.RefundRequired;
        }
    }
}