using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Services;

public class TicketValidationService
{
    public void ValidateAndCheckIn(Ticket ticket, Event @event)
    {
        if (ticket.EventId != @event.Id)
            throw new InvalidOperationException("Ticket does not match the event.");

        if (@event.Status == EventStatus.Cancelled)
            throw new InvalidOperationException("Event has been cancelled. Cannot check in.");

        ticket.CheckIn();
    }
}
