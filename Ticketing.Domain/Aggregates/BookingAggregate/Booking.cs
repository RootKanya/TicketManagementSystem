using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Aggregates.BookingAggregate;

public class Booking
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid EventId { get; private set; }
    public Guid TicketCategoryId { get; private set; }
    public int Quantity { get; private set; }
    public Money TotalPrice { get; private set; }
    public string Status { get; private set; }
    public DateTime PaymentDeadline { get; private set; }

    public Booking(Guid customerId, Guid eventId, Guid categoryId, int quantity, Money unitPrice)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        EventId = eventId;
        TicketCategoryId = categoryId;
        Quantity = quantity;
        TotalPrice = unitPrice * quantity;
        Status = "PendingPayment";
        PaymentDeadline = DateTime.UtcNow.AddMinutes(15);
    }

    public void CompletePayment()
    {
        if (DateTime.UtcNow > PaymentDeadline)
            throw new InvalidOperationException("Payment deadline has passed.");

        Status = "Paid";
    }
}
