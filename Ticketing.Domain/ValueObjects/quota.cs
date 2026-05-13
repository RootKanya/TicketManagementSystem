using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.ValueObjects;

public record Quota
{
    public int Value { get; init; }

    public Quota(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Capacity or quantity must be greater than zero.");

        Value = value;
    }

    public static implicit operator int(Quota q) => q.Value;
    public static implicit operator Quota(int v) => new(v);
}
