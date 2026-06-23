using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.ValueObjects;

public record SalesPeriod
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public SalesPeriod(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("Sales end date cannot be earlier than sales start date.");
        }

        StartDate = startDate;
        EndDate = endDate;
    }

    public bool IsValidAgainstEventStartDate(DateTime eventStartDate)
    {
        return EndDate <= eventStartDate;
    }
}
