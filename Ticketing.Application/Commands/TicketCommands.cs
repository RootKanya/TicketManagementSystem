using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System;

namespace Ticketing.Application.Commands
{
    public class CheckInTicketCommand : IRequest<bool>
    {
        public string TicketCode { get; set; }
        public Guid EventId { get; set; }
    }
}
