using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Repositories;

namespace Ticketing.Infrastructure.Persistence
{
    public class RefundRepository : IRefundRepository
    {
        private readonly TicketingDbContext _context;

        public RefundRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Refund?> GetByIdAsync(Guid id) => await _context.Refunds.FindAsync(id);

        public async Task<Refund?> GetByBookingIdAsync(Guid bookingId) =>
            await _context.Refunds.FirstOrDefaultAsync(r => r.BookingId == bookingId);

        public async Task AddAsync(Refund refund)
        {
            await _context.Refunds.AddAsync(refund);
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Refund refund)
        {
            _context.Refunds.Update(refund);
            return _context.SaveChangesAsync();
        }
    }
}