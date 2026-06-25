using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ticketing.Application.Commands;
using Ticketing.Application.Queries;

namespace Ticketing.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("purchased/{customerId}")]
        public async Task<IActionResult> GetPurchasedTickets(Guid customerId)
        {
            var query = new ViewPurchasedTicketsQuery { CustomerId = customerId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInTicketCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok(new { Message = "Check-in successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}