using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Aggregates;

public class Event
{
    private readonly List<TicketCategory> _ticketCategories = new();
    private readonly List<object> _domainEvents = new();

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public EventSchedule Schedule { get; private set; }
    public string Location { get; private set; }
    public EventCapacity Capacity { get; private set; }
    public EventStatus Status { get; private set; }
    public IReadOnlyCollection<TicketCategory> TicketCategories => _ticketCategories.AsReadOnly();
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    public Event(Guid id, string name, string description, EventSchedule schedule, string location, EventCapacity capacity)
    {
        Id = id;
        Name = name;
        Description = description;
        Schedule = schedule;
        Location = location;
        Capacity = capacity;
        Status = EventStatus.Draft;

        _domainEvents.Add(new { EventId = Id, OccurredOn = DateTime.UtcNow });
    }

    public void AddTicketCategory(string name, Money price, TicketQuantity quota, SalesPeriod salesPeriod)
    {
        if (!salesPeriod.IsValidAgainstEventStartDate(Schedule.StartDate))
        {
            throw new ArgumentException("The ticket sales period must end before or at the event start date.");
        }

        int currentTotalQuota = _ticketCategories.Sum(c => c.Quota.Value);
        if (currentTotalQuota + quota.Value > Capacity.Value)
        {
            throw new ArgumentException("The total quota of all ticket categories must not exceed the maximum event capacity.");
        }

        var category = new TicketCategory(Guid.NewGuid(), name, price, quota, salesPeriod);
        _ticketCategories.Add(category);

        _domainEvents.Add(new { EventId = Id, CategoryId = category.Id });
    }

    public void Publish()
    {
        if (Status == EventStatus.Cancelled)
        {
            throw new InvalidOperationException("An event with the status Cancelled cannot be published.");
        }

        if (!_ticketCategories.Any(c => c.IsActive))
        {
            throw new InvalidOperationException("An event can only be published if it has at least one active ticket category.");
        }

        int totalQuota = _ticketCategories.Where(c => c.IsActive).Sum(c => c.Quota.Value);
        if (totalQuota > Capacity.Value)
        {
            throw new InvalidOperationException("Total ticket quota does not exceed the maximum event capacity.");
        }

        Status = EventStatus.Published;
        _domainEvents.Add(new { EventId = Id });
    }

    public void Cancel()
    {
        if (Status == EventStatus.Completed)
        {
            throw new InvalidOperationException("An event with the status Completed cannot be cancelled.");
        }

        Status = EventStatus.Cancelled;
        _domainEvents.Add(new { EventId = Id }); 
    }

    public void DisableTicketCategory(Guid categoryId)
    {
        if (Status == EventStatus.Completed)
        {
            throw new InvalidOperationException("A ticket category can be disabled if the event has not been completed.");
        }

        var category = _ticketCategories.FirstOrDefault(c => c.Id == categoryId);
        if (category != null)
        {
            category.Disable();
            _domainEvents.Add(new { EventId = Id, CategoryId = categoryId });
        }
    }

    public void ClearEvents() => _domainEvents.Clear();
}