using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System;

namespace Ticketing.Application.Commands
{
    public class RequestRefundCommand : IRequest<Guid>
    {
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
    }

    public class ApproveRefundCommand : IRequest<bool>
    {
        public Guid RefundId { get; set; }
    }

    public class RejectRefundCommand : IRequest<bool>
    {
        public Guid RefundId { get; set; }
        public string Reason { get; set; }
    }

    public class MarkRefundPaidOutCommand : IRequest<bool>
    {
        public Guid RefundId { get; set; }
        public string TargetAccount { get; set; } 
    }
}