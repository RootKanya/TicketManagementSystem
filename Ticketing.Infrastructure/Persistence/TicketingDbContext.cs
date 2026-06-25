using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Persistence
{
    public class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Refund> Refunds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.TicketCode).IsRequired().HasMaxLength(20);
            });

            modelBuilder.Entity<Refund>(entity =>
            {
                entity.HasKey(r => r.Id);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.OwnsOne(b => b.TotalPrice);
                entity.Ignore(b => b.DomainEvents);
            });
        }
    }
}