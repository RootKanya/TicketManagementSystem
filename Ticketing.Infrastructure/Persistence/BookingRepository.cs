using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Enums;
using Ticketing.Domain.Repositories;

namespace Ticketing.Infrastructure.Persistence
{
    public class BookingRepository : IBookingRepository
    {
        private readonly TicketingDbContext _context;

        public BookingRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _context.Bookings.FindAsync(new object[] { id }, cancellationToken);

        public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            await _context.Bookings.AddAsync(booking, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> HasActiveBookingForEventAsync(Guid customerId, Guid eventId, CancellationToken cancellationToken = default) =>
            await _context.Bookings.AnyAsync(b =>
                b.CustomerId == customerId &&
                b.EventId == eventId &&
                b.Status == BookingStatus.PendingPayment,
                cancellationToken);
    }
}