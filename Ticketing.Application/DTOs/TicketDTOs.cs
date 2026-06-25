using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.DTOs
{
    public class PurchasedTicketDto
    {
        public Guid TicketId { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; } // Kita join ke tabel Event nanti
        public string TicketCode { get; set; }
        public string Status { get; set; }
    }
}