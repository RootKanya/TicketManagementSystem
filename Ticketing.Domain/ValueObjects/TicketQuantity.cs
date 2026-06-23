using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.ValueObjects;

public record TicketQuantity
{
    public int Value { get; init; }

    public TicketQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Ticket quantity/quota must be greater than zero.");
        }

        Value = quantity;
    }

    // Operator overloads
    public static TicketQuantity operator +(TicketQuantity a, TicketQuantity b) => new TicketQuantity(a.Value + b.Value);
    public static TicketQuantity operator -(TicketQuantity a, TicketQuantity b) => new TicketQuantity(a.Value - b.Value);
    public static bool operator >(TicketQuantity a, TicketQuantity b) => a.Value > b.Value;
    public static bool operator <(TicketQuantity a, TicketQuantity b) => a.Value < b.Value;
    public static bool operator >=(TicketQuantity a, TicketQuantity b) => a.Value >= b.Value;
    public static bool operator <=(TicketQuantity a, TicketQuantity b) => a.Value <= b.Value;
}
