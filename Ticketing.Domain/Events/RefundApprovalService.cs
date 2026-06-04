using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Services;

public class RefundApprovalService
{
    public void ApproveRefund(Refund refund, Booking booking, IEnumerable<Ticket> tickets)
    {
        if (refund.BookingId != booking.Id)
            throw new InvalidOperationException("Refund does not match this booking.");

        refund.Approve();
        booking.SetStatusForTesting(BookingStatus.Refunded); 
        foreach (var ticket in tickets)
        {
            ticket.Cancel(); 
        }
    }
}