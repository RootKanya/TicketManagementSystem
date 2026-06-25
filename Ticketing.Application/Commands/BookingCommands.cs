using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.Commands;

public record CreateTicketBookingCommand(Guid CustomerId, Guid EventId, Guid CategoryId, int Quantity);

public class ExpireBookingCommand : IRequest<bool>
{
    public Guid BookingId { get; set; }
}