using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Aggregates.RefundAggregate;

public class Refund
{
    public Guid Id { get; private set; }
    public Guid BookingId { get; private set; }
    public string Status { get; private set; } 
    public string? RejectionReason { get; private set; }
    public string? PaymentReference { get; private set; }

    public Refund(Guid bookingId)
    {
        Id = Guid.NewGuid();
        BookingId = bookingId;
        Status = "Requested"; 
    }

    public void Approve()
    {
        [cite_start]// A refund can only be approved if its status is Requested [cite: 211]
        if (Status != "Requested")
            throw new InvalidOperationException("Only Requested refunds can be approved.");

        Status = "Approved"; // [cite: 212]
    }

    public void Reject(string reason)
    {
        if (Status != "Requested")
            throw new InvalidOperationException("Only Requested refunds can be rejected.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("A rejection reason is required for processing.");

        Status = "Rejected";
        RejectionReason = reason;
    }

    public void MarkAsPaidOut(string reference)
    {
        if (Status != "Approved")
            throw new InvalidOperationException("Only Approved refunds can be marked as paid out.");

        if (string.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("Payment reference is mandatory for paid out status.");

        Status = "PaidOut"; 
        PaymentReference = reference;
    }
}
