using System;
using System.Collections.Generic;
using System.Linq;
using Ticketing.Domain.Enums;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Aggregates
{
    public class Booking
    {
        private readonly List<object> _domainEvents = new();

        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid EventId { get; private set; }
        public Guid TicketCategoryId { get; private set; }
        public TicketQuantity Quantity { get; private set; }
        public Money TotalPrice { get; private set; }
        public BookingStatus Status { get; private set; }
        public DateTime PaymentDeadline { get; private set; }
        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        public Booking(Guid id, Guid customerId, Event @event, Guid categoryId, TicketQuantity quantity, decimal serviceFee)
        {
            if (@event.Status != EventStatus.Published)
            {
                throw new InvalidOperationException("A booking can only be created for an event with the status Published.");
            }

            var category = @event.TicketCategories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null || !category.IsActive)
            {
                throw new InvalidOperationException("A booking can only be created for an active ticket category.");
            }

            DateTime now = DateTime.UtcNow;
            if (now < category.SalesPeriod.StartDate || now > category.SalesPeriod.EndDate)
            {
                throw new InvalidOperationException("A booking can only be created within the ticket sales period.");
            }

            category.ReserveSeats(quantity.Value);

            Id = id;
            CustomerId = customerId;
            EventId = @event.Id;
            TicketCategoryId = categoryId;
            Quantity = quantity;
            Status = BookingStatus.PendingPayment;
            PaymentDeadline = now.AddMinutes(15);

            decimal basePrice = category.Price.Amount * quantity.Value;
            decimal finalAmount = basePrice + serviceFee;
            TotalPrice = new Money(finalAmount, category.Price.Currency);

            _domainEvents.Add(new { BookingId = Id, CustomerId = CustomerId });
        }

        public void ClearEvents() => _domainEvents.Clear();

        public void Pay()
        {
            if (Status != BookingStatus.PendingPayment)
                throw new InvalidOperationException("Only pending bookings can be paid.");

            Status = BookingStatus.Paid;
        }

        public void Expire()
        {
            if (Status != BookingStatus.PendingPayment)
                throw new InvalidOperationException("Only pending bookings can expire.");

            Status = BookingStatus.Expired;
        }

        public void MarkAsRefunded()
        {
            if (Status != BookingStatus.Paid)
                throw new InvalidOperationException("Only paid bookings can be refunded.");

            Status = BookingStatus.Refunded;
        }
    }
}