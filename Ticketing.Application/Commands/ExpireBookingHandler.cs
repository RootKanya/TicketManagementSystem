using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ticketing.Application.Commands;
using Ticketing.Domain.Repositories;

namespace Ticketing.Application.CommandHandlers
{
    public class ExpireBookingHandler : IRequestHandler<ExpireBookingCommand, bool>
    {
        private readonly IBookingRepository _bookingRepository;

        public ExpireBookingHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> Handle(ExpireBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);
            if (booking == null)
                throw new Exception("Booking not found.");

            booking.Expire();

            await _bookingRepository.UpdateAsync(booking);

            return true;
        }
    }
}