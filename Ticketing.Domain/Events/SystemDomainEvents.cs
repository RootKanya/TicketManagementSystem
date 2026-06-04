using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Ticketing.Domain.Events;

// --- Event Management Events ---
public record EventCreated(Guid EventId) : IDomainEvent;
public record EventPublished(Guid EventId) : IDomainEvent;
public record EventCancelled(Guid EventId) : IDomainEvent;

// --- Ticket Category Events ---
public record TicketCategoryCreated(Guid CategoryId, Guid EventId) : IDomainEvent;
public record TicketCategoryDisabled(Guid CategoryId) : IDomainEvent;

// --- Booking Events ---
public record TicketReserved(Guid BookingId, Guid CustomerId) : IDomainEvent;
public record BookingPaid(Guid BookingId) : IDomainEvent;
public record BookingExpired(Guid BookingId) : IDomainEvent;

// --- Ticket Events ---
public record TicketCheckedIn(Guid TicketId, string TicketCode) : IDomainEvent;

// --- Refund Events ---
public record RefundRequested(Guid RefundId, Guid BookingId) : IDomainEvent;
public record RefundApproved(Guid RefundId) : IDomainEvent;
public record RefundRejected(Guid RefundId, string Reason) : IDomainEvent;
public record RefundPaidOut(Guid RefundId) : IDomainEvent;