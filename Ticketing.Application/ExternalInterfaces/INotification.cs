using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.ExternalInterfaces;

public interface INotificationService
{
    Task SendBookingConfirmationAsync(Guid customerId, Guid bookingId, CancellationToken cancellationToken);
}
