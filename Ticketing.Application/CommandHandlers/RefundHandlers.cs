using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ticketing.Application.Commands;
using Ticketing.Application.ExternalInterfaces;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Repositories;

namespace Ticketing.Application.CommandHandlers
{
    public class RequestRefundHandler : IRequestHandler<RequestRefundCommand, Guid>
    {
        private readonly IRefundRepository _refundRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IBookingRepository _bookingRepository;

        public RequestRefundHandler(IRefundRepository refundRepo, ITicketRepository ticketRepo, IBookingRepository bookingRepo)
        {
            _refundRepository = refundRepo;
            _ticketRepository = ticketRepo;
            _bookingRepository = bookingRepo;
        }

        public async Task<Guid> Handle(RequestRefundCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

            var tickets = await _ticketRepository.GetByBookingIdAsync(request.BookingId);

            if (tickets.Any(t => t.Status == Ticketing.Domain.Enums.TicketStatus.CheckedIn))
                throw new InvalidOperationException("Cannot request refund because one or more tickets have already been checked in.");

            var refund = new Refund(request.BookingId, request.CustomerId);
            await _refundRepository.AddAsync(refund);

            return refund.Id;
        }
    }

    public class ApproveRefundHandler : IRequestHandler<ApproveRefundCommand, bool>
    {
        private readonly IRefundRepository _refundRepository;

        public ApproveRefundHandler(IRefundRepository refundRepository)
        {
            _refundRepository = refundRepository;
        }

        public async Task<bool> Handle(ApproveRefundCommand request, CancellationToken cancellationToken)
        {
            var refund = await _refundRepository.GetByIdAsync(request.RefundId);
            if (refund == null) throw new Exception("Refund not found.");

            refund.Approve();
            await _refundRepository.UpdateAsync(refund);

            return true;
        }
    }

    public class RejectRefundHandler : IRequestHandler<RejectRefundCommand, bool>
    {
        private readonly IRefundRepository _refundRepository;

        public RejectRefundHandler(IRefundRepository refundRepository)
        {
            _refundRepository = refundRepository;
        }

        public async Task<bool> Handle(RejectRefundCommand request, CancellationToken cancellationToken)
        {
            var refund = await _refundRepository.GetByIdAsync(request.RefundId);
            if (refund == null) throw new Exception("Refund not found.");

            refund.Reject(request.Reason);
            await _refundRepository.UpdateAsync(refund);

            return true;
        }
    }

    public class MarkRefundPaidOutHandler : IRequestHandler<MarkRefundPaidOutCommand, bool>
    {
        private readonly IRefundRepository _refundRepository;
        private readonly IRefundService _refundService;

        public MarkRefundPaidOutHandler(IRefundRepository refundRepository, IRefundService refundService)
        {
            _refundRepository = refundRepository;
            _refundService = refundService;
        }

        public async Task<bool> Handle(MarkRefundPaidOutCommand request, CancellationToken cancellationToken)
        {
            var refund = await _refundRepository.GetByIdAsync(request.RefundId);
            if (refund == null) throw new Exception("Refund not found.");

            var paymentReference = await _refundService.ProcessRefundPayoutAsync(refund.Id, 500000m, request.TargetAccount);

            refund.MarkAsPaidOut(paymentReference);
            await _refundRepository.UpdateAsync(refund);

            return true;
        }
    }
}