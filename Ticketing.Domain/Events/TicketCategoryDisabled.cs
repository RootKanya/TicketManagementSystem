using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Events;

public record TicketCategoryDisabled(Guid EventId, Guid CategoryId, DateTime OccurredOn);
