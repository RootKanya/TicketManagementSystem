using System;
using System.Threading.Tasks;
using Ticketing.Domain.Aggregates;

namespace Ticketing.Domain.Repositories
{
    public interface IRefundRepository
    {
        Task<Refund?> GetByIdAsync(Guid id);
        Task<Refund?> GetByBookingIdAsync(Guid bookingId);
        Task AddAsync(Refund refund);
        Task UpdateAsync(Refund refund);
    }
}