using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.Shared;

public interface IPaymentService
{
    Task<bool> ProcessPaymentAsync(Guid bookingId, decimal amount, CancellationToken cancellationToken = default);
}

public interface INotificationService
{
    Task SendTicketNotificationAsync(Guid customerId, string ticketCode, CancellationToken cancellationToken = default);
}
