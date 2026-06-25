using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.Commands;

public record CreateEventCommand(string Name, string Description, DateTime StartDate, DateTime EndDate, string Location, int Capacity);
public record PublishEventCommand(Guid EventId);
public record CancelEventCommand(Guid EventId);
