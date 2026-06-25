using MediatR;
using System;
using System.Collections.Generic;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.Queries
{
    public class ViewPurchasedTicketsQuery : IRequest<IEnumerable<PurchasedTicketDto>>
    {
        public Guid CustomerId { get; set; }
    }
}