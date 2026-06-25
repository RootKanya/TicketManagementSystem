using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.Commands;

public record CreateTicketCategoryCommand(Guid EventId, string Name, decimal Price, string Currency, int Quota, DateTime SalesStartDate, DateTime SalesEndDate);
public record DisableTicketCategoryCommand(Guid EventId, Guid CategoryId);