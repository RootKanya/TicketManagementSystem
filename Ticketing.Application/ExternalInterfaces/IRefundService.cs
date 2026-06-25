using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.ExternalInterfaces
{
    public interface IRefundService
    {
        Task<string> ProcessRefundPayoutAsync(Guid refundId, decimal amount, string targetAccount);
    }
}