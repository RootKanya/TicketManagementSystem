using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.ExternalInterfaces;

public interface IPaymentGatewayService
{
    Task<bool> ProcessPaymentAsync(Guid bookingId, decimal amount, string currency, CancellationToken cancellationToken);
}