using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Enums
{
    public enum TicketStatus
    {
        Active,
        CheckedIn,
        Cancelled,
        RefundRequired
    }
}