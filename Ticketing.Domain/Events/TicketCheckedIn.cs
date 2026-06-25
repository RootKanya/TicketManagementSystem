using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Events
{
    public class TicketCheckedIn 
    {
        public Guid TicketId { get; }
        public Guid EventId { get; }
        public DateTime CheckedInAt { get; }

        public TicketCheckedIn(Guid ticketId, Guid eventId)
        {
            TicketId = ticketId;
            EventId = eventId;
            CheckedInAt = DateTime.UtcNow;
        }
    }
}