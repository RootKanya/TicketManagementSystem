using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.ValueObjects;

public record EventCapacity
{
    public int Value { get; init; }

    public EventCapacity(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("The event cannot be created if the maximum capacity is less than or equal to zero.");
        }

        Value = capacity;
    }
}