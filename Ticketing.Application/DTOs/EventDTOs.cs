using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.DTOs;

public record EventListItemDto(Guid Id, string Name, DateTime Date, string Location, decimal LowestPrice);
public record EventDetailDto(Guid Id, string Name, string Description, DateTime Date, string Location, List<TicketCategoryDto> Categories);
public record TicketCategoryDto(Guid Id, string Name, decimal Price, string Status);