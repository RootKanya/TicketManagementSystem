using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ticketing.Application.Commands;
using Ticketing.Domain.Repositories;

namespace Ticketing.Application.CommandHandlers;

public class DisableTicketCategoryCommandHandler : IRequestHandler<DisableTicketCategoryCommand>
{
    private readonly IEventRepository _eventRepository;

    public DisableTicketCategoryCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(DisableTicketCategoryCommand command, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(command.EventId, cancellationToken)
            ?? throw new KeyNotFoundException($"Event with ID {command.EventId} was not found.");

        @event.DisableTicketCategory(command.CategoryId);

        await _eventRepository.UpdateAsync(@event, cancellationToken);
    }
}