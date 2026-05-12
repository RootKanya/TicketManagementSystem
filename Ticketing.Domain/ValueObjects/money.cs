using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.ValueObjects;
public record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency = "IDR") => new(0, currency);
    public static Money Create(decimal amount, string currency = "IDR")
    {
        if (amount < 0)
            throw new ArgumentException("Price cannot be negative.");

        return new Money(amount, currency);
    }
}
