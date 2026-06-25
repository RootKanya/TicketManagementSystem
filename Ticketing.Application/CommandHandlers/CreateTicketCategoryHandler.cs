using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.CommandHandlers;

using global::Ticketing.Application.Commands;
using global::Ticketing.Domain.Repositories;
using global::Ticketing.Domain.ValueObjects;

public class CreateTicketCategoryCommandHandler
{
    private readonly IEventRepository _eventRepository;

    public CreateTicketCategoryCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(CreateTicketCategoryCommand command, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(command.EventId, cancellationToken)
            ?? throw new KeyNotFoundException($"Event with ID {command.EventId} was not found.");

        var price = new Money(command.Price, command.Currency);
        var quota = new TicketQuantity(command.Quota);
        var salesPeriod = new SalesPeriod(command.SalesStartDate, command.SalesEndDate);

        @event.AddTicketCategory(command.Name, price, quota, salesPeriod);

        await _eventRepository.UpdateAsync(@event, cancellationToken);
    }
}
