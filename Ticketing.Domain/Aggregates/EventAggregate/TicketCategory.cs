using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Aggregates.EventAggregate;

public class TicketCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Money Price { get; private set; }
    public EventCapacity Quota { get; private set; }
    public EventSchedule SalesPeriod { get; private set; }
    public bool IsActive { get; private set; }

    public TicketCategory(string name, Money price, EventCapacity quota, EventSchedule salesPeriod)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Quota = quota;
        SalesPeriod = salesPeriod;
        IsActive = true;
    }

    public void Disable()
    {
        IsActive = false;
    }
}
