using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Enums;
using Ticketing.Domain.Events;

namespace Ticketing.Domain.Aggregates
{
    public class Refund
    {
        public Guid Id { get; private set; }
        public Guid BookingId { get; private set; }
        public Guid CustomerId { get; private set; }
        public RefundStatus Status { get; private set; }
        public string RejectionReason { get; private set; }
        public string PaymentReference { get; private set; }
        public DateTime RequestedAt { get; private set; }

        private Refund() { }

        public Refund(Guid bookingId, Guid customerId)
        {
            Id = Guid.NewGuid();
            BookingId = bookingId;
            CustomerId = customerId;
            Status = RefundStatus.Requested;
            RequestedAt = DateTime.UtcNow;
        }

        public void Approve()
        {
            if (Status != RefundStatus.Requested)
                throw new InvalidOperationException("Only requested refunds can be approved.");

            Status = RefundStatus.Approved;

        }

        public void Reject(string reason)
        {
            if (Status != RefundStatus.Requested)
                throw new InvalidOperationException("Only requested refunds can be rejected.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Rejection reason must be provided.");

            Status = RefundStatus.Rejected;
            RejectionReason = reason;

        }

        public void MarkAsPaidOut(string paymentReference)
        {
            if (Status != RefundStatus.Approved)
                throw new InvalidOperationException("Only approved refunds can be paid out.");

            if (string.IsNullOrWhiteSpace(paymentReference))
                throw new ArgumentException("Payment reference must be provided.");

            Status = RefundStatus.PaidOut;
            PaymentReference = paymentReference;

        }
    }
}