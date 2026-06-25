using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Commands;

namespace Ticketing.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketBookingCommand command)
    {
        try
        {
            var bookingId = await _mediator.Send(command);
            return Ok(new { BookingId = bookingId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("{id}/expire")]
    public async Task<IActionResult> Expire(Guid id)
    {
        var command = new ExpireBookingCommand { BookingId = id };

        try
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Booking marked as expired." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}