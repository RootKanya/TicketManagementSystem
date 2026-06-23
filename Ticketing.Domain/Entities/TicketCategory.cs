using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Entities;

public class TicketCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Money Price { get; private set; }
    public TicketQuantity Quota { get; private set; }
    public SalesPeriod SalesPeriod { get; private set; }
    public bool IsActive { get; private set; }
    public int ReservedQuantity { get; private set; }

    public TicketCategory(Guid id, string name, Money price, TicketQuantity quota, SalesPeriod salesPeriod)
    {
        Id = id;
        Name = name;
        Price = price;
        Quota = quota;
        SalesPeriod = salesPeriod;
        IsActive = true;
        ReservedQuantity = 0;
    }

    public int RemainingQuota => Quota.Value - ReservedQuantity;

    public void Disable()
    {
        IsActive = false;
    }

    public void ReserveSeats(int quantity)
    {
        if (quantity > RemainingQuota)
        {
            throw new ArgumentException("The ticket quantity must not exceed the remaining ticket quota.");
        }
        ReservedQuantity += quantity;
    }

    public void ReleaseSeats(int quantity)
    {
        ReservedQuantity -= quantity;
    }
}
