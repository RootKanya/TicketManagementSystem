using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.ValueObjects;

public record TicketCode
{
    public string Value { get; init; }

    public TicketCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Ticket code cannot be empty.");

        Value = value;
    }

    public static TicketCode Generate() => new(Guid.NewGuid().ToString("N").ToUpper().Substring(0, 10));
}
