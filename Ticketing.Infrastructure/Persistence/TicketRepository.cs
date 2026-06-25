using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Repositories;
using Ticketing.Infrastructure.Data; 

namespace Ticketing.Infrastructure.Persistence
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketingDbContext _context;

        public TicketRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket?> GetByIdAsync(Guid id) =>
            await _context.Tickets.FindAsync(id);

        public async Task<Ticket?> GetByCodeAsync(string ticketCode) =>
            await _context.Tickets.FirstOrDefaultAsync(t => t.TicketCode == ticketCode);

        public async Task<IEnumerable<Ticket>> GetByBookingIdAsync(Guid bookingId) =>
            await _context.Tickets.Where(t => t.BookingId == bookingId).ToListAsync();

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Ticket> tickets)
        {
            await _context.Tickets.AddRangeAsync(tickets);
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            return _context.SaveChangesAsync();
        }

        public Task UpdateRangeAsync(IEnumerable<Ticket> tickets)
        {
            _context.Tickets.UpdateRange(tickets);
            return _context.SaveChangesAsync();
        }
    }
}