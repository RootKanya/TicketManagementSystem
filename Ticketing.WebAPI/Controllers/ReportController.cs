using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Queries;

namespace Ticketing.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{eventId}/sales")]
    public async Task<IActionResult> GetSalesReport(Guid eventId)
    {
        try
        {
            var query = new GetEventSalesReportQuery(eventId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("{eventId}/participants")]
    public async Task<IActionResult> GetParticipants(Guid eventId)
    {
        try
        {
            var query = new GetEventParticipantsQuery(eventId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}