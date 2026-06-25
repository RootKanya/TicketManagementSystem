using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Repositories;

namespace Ticketing.Infrastructure.Persistence
{
    public class EventRepository : IEventRepository
    {
        private readonly TicketingDbContext _context;

        public EventRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _context.Events
                .Include(e => e.TicketCategories)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public async Task AddAsync(Event @event, CancellationToken cancellationToken = default)
        {
            await _context.Events.AddAsync(@event, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Event @event, CancellationToken cancellationToken = default)
        {
            _context.Events.Update(@event);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}