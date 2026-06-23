using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Aggregates.EventAggregate;

public class Event
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public EventSchedule Schedule { get; private set; }
    public EventCapacity MaxCapacity { get; private set; } 
    public string Status { get; private set; }

    private readonly List<TicketCategory> _categories = new();
    public IReadOnlyCollection<TicketCategory> Categories => _categories.AsReadOnly();

    public Event(string name, string description, EventSchedule schedule, EventCapacity maxCapacity)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Schedule = schedule;
        MaxCapacity = maxCapacity;
        Status = "Draft"; 
    }

    public void AddTicketCategory(string name, Money price, EventCapacity quota, EventSchedule salesPeriod)
    {
        if (_categories.Sum(c => c.Quota.Value) + quota.Value > MaxCapacity.Value)
            throw new InvalidOperationException("Total quota exceeds maximum event capacity.");

        _categories.Add(new TicketCategory(name, price, quota, salesPeriod));
    }

    public void Publish()
    {
        if (!_categories.Any(c => c.IsActive))
            throw new InvalidOperationException("Event must have at least one active ticket category.");

        if (Status == "Cancelled")
            throw new InvalidOperationException("Cancelled events cannot be published.");

        Status = "Published";
    }
}