using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Repositories;

namespace Ticketing.Infrastructure.Persistence
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketingDbContext _context;

        public TicketRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _context.Tickets.FindAsync(new object[] { id }, cancellationToken);

        public async Task<Ticket?> GetByCodeAsync(string ticketCode, CancellationToken cancellationToken = default) =>
            await _context.Tickets.FirstOrDefaultAsync(t => t.TicketCode == ticketCode, cancellationToken);

        public async Task<IEnumerable<Ticket>> GetByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken = default) =>
            await _context.Tickets.Where(t => t.BookingId == bookingId).ToListAsync(cancellationToken);

        public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default)
        {
            await _context.Tickets.AddAsync(ticket, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<Ticket> tickets, CancellationToken cancellationToken = default)
        {
            await _context.Tickets.AddRangeAsync(tickets, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken = default)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRangeAsync(IEnumerable<Ticket> tickets, CancellationToken cancellationToken = default)
        {
            _context.Tickets.UpdateRange(tickets);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}