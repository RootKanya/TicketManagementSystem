using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.Queries;

public record GetAvailableEventsQuery(DateTime? FilterDate, string? FilterLocation);

public record GetEventDetailsQuery(Guid EventId);
