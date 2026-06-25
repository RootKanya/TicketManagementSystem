using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Ticketing.Application.Commands;

public record CreateEventCommand(
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    int Capacity) : IRequest<Guid>;

public record PublishEventCommand(Guid EventId) : IRequest;

public record CancelEventCommand(Guid EventId) : IRequest;