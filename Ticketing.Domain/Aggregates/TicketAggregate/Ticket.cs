using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;


namespace Ticketing.Domain.Aggregates.TicketAggregate;

public class Ticket
{
    public Guid Id { get; private set; }
    public Guid BookingId { get; private set; }
    public Guid EventId { get; private set; }
    public Guid TicketCategoryId { get; private set; }
    public SalesPeriod Code { get; private set; } 
    public string Status { get; private set; } 

    public Ticket(Guid bookingId, Guid eventId, Guid categoryId, SalesPeriod code)
    {
        Id = Guid.NewGuid();
        BookingId = bookingId;
        EventId = eventId;
        TicketCategoryId = categoryId;
        Code = code;
        Status = "Active"; 
    }

    public void CheckIn(Guid scanningEventId)
    {
        if (scanningEventId != EventId)
            throw new InvalidOperationException("This ticket does not match the event being scanned.");

        if (Status != "Active")
        {
            if (Status == "CheckedIn")
                throw new InvalidOperationException("This ticket has already been used.");
            
            throw new InvalidOperationException($"Cannot check in. Ticket status is {Status}.");
        }

        Status = "CheckedIn";
    }

    public void Cancel()
    {
        Status = "Cancelled";
    }
}
