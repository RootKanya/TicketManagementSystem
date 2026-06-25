using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ticketing.Application.Commands; 
using Ticketing.Application.Queries;

namespace Ticketing.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventCommand command)
    {
        var eventId = await _mediator.Send(command);
        return Ok(new { EventId = eventId });
    }

    [HttpPost("{id}/publish")]
    public async Task<IActionResult> Publish(Guid id)
    {
        await _mediator.Send(new PublishEventCommand(id));
        return Ok(new { Message = "Event published successfully." });
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _mediator.Send(new CancelEventCommand(id));
        return Ok(new { Message = "Event cancelled successfully." });
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailable([FromQuery] DateTime? filterDate, [FromQuery] string? filterLocation)
    {
        try
        {
            var query = new GetAvailableEventsQuery(filterDate, filterLocation);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        try
        {
            var query = new GetEventDetailsQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { Message = "Event not found." });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}