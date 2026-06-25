using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.CommandHandlers;

using global::Ticketing.Application.Commands;
using global::Ticketing.Domain.Repositories;
using MediatR;

public class PublishEventHandler : IRequestHandler<PublishEventCommand>
{
    private readonly IEventRepository _eventRepository;

    public PublishEventHandler(IEventRepository eventRepository) => _eventRepository = eventRepository;

    public async Task Handle(PublishEventCommand command, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(command.EventId, cancellationToken)
            ?? throw new KeyNotFoundException("Event not found.");

        @event.Publish();

        await _eventRepository.UpdateAsync(@event, cancellationToken);
    }
}
