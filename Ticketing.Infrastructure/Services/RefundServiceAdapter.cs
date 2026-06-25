using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Application.ExternalInterfaces;

namespace Ticketing.Infrastructure.Services 
{
    public class RefundServiceAdapter : IRefundService
    {
        public Task<string> ProcessRefundPayoutAsync(Guid refundId, decimal amount, string targetAccount)
        {
            var mockPaymentReference = $"REF-BANK-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            Console.WriteLine($"[Mock Bank API] Payout for Refund {refundId} to {targetAccount} completed. Ref: {mockPaymentReference}");

            return Task.FromResult(mockPaymentReference);
        }
    }
}
