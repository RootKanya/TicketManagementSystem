using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ticketing.Application.Commands;

namespace Ticketing.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefundsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RefundsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestRefund([FromBody] RequestRefundCommand command)
        {
            try
            {
                var refundId = await _mediator.Send(command);
                return Ok(new { RefundId = refundId, Message = "Refund requested successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("{refundId}/approve")]
        public async Task<IActionResult> ApproveRefund(Guid refundId)
        {
            try
            {
                var command = new ApproveRefundCommand { RefundId = refundId };
                await _mediator.Send(command);
                return Ok(new { Message = "Refund approved." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("{refundId}/reject")]
        public async Task<IActionResult> RejectRefund(Guid refundId, [FromBody] RejectRefundRequest requestBody)
        {
            try
            {
                var command = new RejectRefundCommand { RefundId = refundId, Reason = requestBody.Reason };
                await _mediator.Send(command);
                return Ok(new { Message = "Refund rejected." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("{refundId}/payout")]
        public async Task<IActionResult> PayoutRefund(Guid refundId, [FromBody] PayoutRefundRequest requestBody)
        {
            try
            {
                var command = new MarkRefundPaidOutCommand { RefundId = refundId, TargetAccount = requestBody.TargetAccount };
                await _mediator.Send(command);
                return Ok(new { Message = "Refund paid out successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }

    public class RejectRefundRequest { public string Reason { get; set; } }
    public class PayoutRefundRequest { public string TargetAccount { get; set; } }
}