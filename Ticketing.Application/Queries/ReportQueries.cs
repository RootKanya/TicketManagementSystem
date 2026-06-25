using System;
using System.Collections.Generic;
using MediatR;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.Queries;

public record GetEventSalesReportQuery(Guid EventId) : IRequest<SalesReportDto>;

public record GetEventParticipantsQuery(Guid EventId) : IRequest<IEnumerable<ParticipantDto>>;