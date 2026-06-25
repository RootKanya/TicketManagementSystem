using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Commands;

namespace Ticketing.WebAPI.Controllers;

[ApiController]
[Route("api/events/{eventId}/[controller]")]
public class TicketCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketCategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(Guid eventId, [FromBody] CreateTicketCategoryCommand command)
    {
        try
        {
            var categoryId = await _mediator.Send(command);
            return Ok(new { TicketCategoryId = categoryId, Message = "Ticket category created successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPatch("{categoryId}/disable")]
    public async Task<IActionResult> Disable(Guid eventId, Guid categoryId)
    {
        try
        {
            var command = new DisableTicketCategoryCommand(eventId, categoryId);

            await _mediator.Send(command);
            return Ok(new { Message = "Ticket category disabled successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}