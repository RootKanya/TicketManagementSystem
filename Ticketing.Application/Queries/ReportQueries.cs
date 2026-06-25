using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.Queries;

public record GetEventSalesReportQuery(Guid EventId);

public record GetEventParticipantsQuery(Guid EventId);
