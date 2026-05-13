using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.ValueObjects;

public record DateRange
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }

    public DateRange(DateTime start, DateTime end)
    {
        if (end < start)
            throw new ArgumentException("End date cannot be earlier than start date.");

        Start = start;
        End = end;
    }

    public bool IsOverlapping(DateTime date) => date >= Start && date <= End;
}
