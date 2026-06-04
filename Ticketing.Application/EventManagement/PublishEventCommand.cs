using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Ticketing.Domain.Repositories;

namespace Ticketing.Application.EventManagement;

public record PublishEventCommand(Guid EventId) : IRequest;

public class PublishEventCommandHandler : IRequestHandler<PublishEventCommand>
{
    private readonly IEventRepository _eventRepository;

    public PublishEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(PublishEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new KeyNotFoundException("Event not found.");

        @event.Publish();
        await _eventRepository.UpdateAsync(@event, cancellationToken);
    }
}
}
