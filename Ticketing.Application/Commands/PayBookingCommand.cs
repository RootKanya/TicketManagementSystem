using MediatR;
using System;

namespace Ticketing.Application.Commands
{
    public class PayBookingCommand : IRequest<bool>
    {
        public Guid BookingId { get; set; }
        public decimal AmountPaid { get; set; }
    }
}