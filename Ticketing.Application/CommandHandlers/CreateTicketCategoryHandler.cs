using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR; 
using Ticketing.Application.Commands;
using Ticketing.Domain.Repositories;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Application.CommandHandlers;

public class CreateTicketCategoryHandler : IRequestHandler<CreateTicketCategoryCommand, Guid>
{
    private readonly IEventRepository _eventRepository;

    public CreateTicketCategoryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Guid> Handle(CreateTicketCategoryCommand command, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(command.EventId, cancellationToken)
            ?? throw new KeyNotFoundException($"Event with ID {command.EventId} was not found.");

        var price = new Money(command.Price, command.Currency);
        var quota = new TicketQuantity(command.Quota);
        var salesPeriod = new SalesPeriod(command.SalesStartDate, command.SalesEndDate);

        var newCategoryId = @event.AddTicketCategory(command.Name, price, quota, salesPeriod);

        await _eventRepository.UpdateAsync(@event, cancellationToken);

        return newCategoryId;
    }
}