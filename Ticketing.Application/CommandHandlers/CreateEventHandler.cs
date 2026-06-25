using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.CommandHandlers;

using global::Ticketing.Application.Commands;
using global::Ticketing.Domain.Aggregates;
using global::Ticketing.Domain.Repositories;
using global::Ticketing.Domain.ValueObjects;

public class CreateEventHandler
{
    private readonly IEventRepository _eventRepository;

    public CreateEventHandler(IEventRepository eventRepository) => _eventRepository = eventRepository;

    public async Task<Guid> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        var @event = new Event(
            Guid.NewGuid(), command.Name, command.Description,
            new EventSchedule(command.StartDate, command.EndDate),
            command.Location, new EventCapacity(command.Capacity)
        );

        await _eventRepository.AddAsync(@event, cancellationToken);
        return @event.Id;
    }
}
