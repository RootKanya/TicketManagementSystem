using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.CommandHandlers;

using global::Ticketing.Application.Commands;
using global::Ticketing.Domain.Repositories;

public class CancelEventCommandHandler
{
    private readonly IEventRepository _eventRepository;

    public CancelEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(CancelEventCommand command, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(command.EventId, cancellationToken)
            ?? throw new KeyNotFoundException($"Event with ID {command.EventId} was not found.");

        @event.Cancel();

        await _eventRepository.UpdateAsync(@event, cancellationToken);
    }
}