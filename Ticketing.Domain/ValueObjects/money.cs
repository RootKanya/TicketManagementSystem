using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public Money(decimal amount, string currency = "IDR")
    {
        if (amount < 0)
            throw new ArgumentException("Price or total price cannot be negative.");

        Amount = amount;
        Currency = currency;
    }

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot add different currencies.");

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator *(Money m, int quantity)
    {
        return new Money(m.Amount * quantity, m.Currency);
    }
}
