using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Persistence
{
    public class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
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

                entity.OwnsOne(b => b.TotalPrice, p =>
                {
                    p.Property(m => m.Amount).HasColumnName("TotalPriceAmount");
                    p.Property(m => m.Currency).HasColumnName("TotalPriceCurrency");
                });

                entity.OwnsOne(b => b.Quantity, q =>
                {
                    q.Property(v => v.Value).HasColumnName("QuantityValue");
                });

                entity.Ignore(b => b.DomainEvents);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);

                entity.OwnsOne(e => e.Schedule, s =>
                {
                    s.Property(p => p.StartDate).HasColumnName("StartDate");
                    s.Property(p => p.EndDate).HasColumnName("EndDate");
                });

                entity.OwnsOne(e => e.Capacity, c =>
                {
                    c.Property(p => p.Value).HasColumnName("Capacity");
                });

                entity.Ignore(e => e.DomainEvents);

                entity.Metadata.FindNavigation(nameof(Event.TicketCategories))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<TicketCategory>(entity =>
            {
                entity.ToTable("TicketCategories");
                entity.HasKey(tc => tc.Id);

                entity.OwnsOne(tc => tc.Price, p =>
                {
                    p.Property(m => m.Amount).HasColumnName("PriceAmount");
                    p.Property(m => m.Currency).HasColumnName("PriceCurrency");
                });

                entity.OwnsOne(tc => tc.Quota, q =>
                {
                    q.Property(v => v.Value).HasColumnName("Quota");
                });

                entity.OwnsOne(tc => tc.SalesPeriod, sp =>
                {
                    sp.Property(p => p.StartDate).HasColumnName("SalesStartDate");
                    sp.Property(p => p.EndDate).HasColumnName("SalesEndDate");
                });
            });
        }
    }
}