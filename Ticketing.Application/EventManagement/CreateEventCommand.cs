using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Ticketing.Domain.Aggregates.EventAggregate;
using Ticketing.Domain.Repositories;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Application.EventManagement;

public record CreateEventCommand(string Name, string Description, DateTime StartDate, DateTime EndDate, string Location, int MaxCapacity) : IRequest<Guid>;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
{
    private readonly IEventRepository _eventRepository;

    public CreateEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var schedule = new DateTimeRange(request.StartDate, request.EndDate);
        var @event = Event.Create(request.Name, request.Description, schedule, request.Location, request.MaxCapacity);

        await _eventRepository.AddAsync(@event, cancellationToken);
        return @event.Id;
    }
}