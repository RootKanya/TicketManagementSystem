using System;
using System.Collections.Generic;
using MediatR;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.Queries;

public record GetAvailableEventsQuery(DateTime? FilterDate, string? FilterLocation) : IRequest<IEnumerable<EventListItemDto>>;

public record GetEventDetailsQuery(Guid EventId) : IRequest<EventDetailDto?>;