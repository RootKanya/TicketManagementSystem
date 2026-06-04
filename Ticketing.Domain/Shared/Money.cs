using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Shared
{
    public class Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency = "IDR")
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.");

            Amount = amount;
            Currency = currency;
        }
        public override bool Equals(object? obj)
        {
            if (obj is Money money)
            {
                return Amount == money.Amount && Currency == money.Currency;
            }
            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    }
}

