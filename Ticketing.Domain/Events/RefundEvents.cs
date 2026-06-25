using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Events
{
    public class RefundRequested
    {
        public Guid RefundId { get; }
        public Guid BookingId { get; }

        public RefundRequested(Guid refundId, Guid bookingId)
        {
            RefundId = refundId;
            BookingId = bookingId;
        }
    }

    public class RefundApproved
    {
        public Guid RefundId { get; }
        public Guid BookingId { get; }

        public RefundApproved(Guid refundId, Guid bookingId)
        {
            RefundId = refundId;
            BookingId = bookingId;
        }
    }

    public class RefundRejected
    {
        public Guid RefundId { get; }
        public Guid BookingId { get; }
        public string Reason { get; }

        public RefundRejected(Guid refundId, Guid bookingId, string reason)
        {
            RefundId = refundId;
            BookingId = bookingId;
            Reason = reason;
        }
    }

    public class RefundPaidOut
    {
        public Guid RefundId { get; }
        public Guid BookingId { get; }

        public RefundPaidOut(Guid refundId, Guid bookingId)
        {
            RefundId = refundId;
            BookingId = bookingId;
        }
    }
}