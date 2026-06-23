using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.ValueObjects;

public record EventSchedule
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public EventSchedule(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("The event cannot be created if the end date is earlier than the start date.");
        }

        StartDate = startDate;
        EndDate = endDate;
    }
}
