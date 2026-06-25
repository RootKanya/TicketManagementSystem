using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ticketing.Application.DTOs;
using Ticketing.Application.ExternalInterfaces;
using Ticketing.Application.Queries;

namespace Ticketing.Application.QueryHandlers
{
    public class ViewPurchasedTicketsHandler : IRequestHandler<ViewPurchasedTicketsQuery, IEnumerable<PurchasedTicketDto>>
    {
        private readonly IQueryConnectionFactory _connectionFactory;

        public ViewPurchasedTicketsHandler(IQueryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<PurchasedTicketDto>> Handle(ViewPurchasedTicketsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    t.""Id"" AS TicketId,
                    t.""EventId"",
                    e.""Title"" AS EventName, 
                    t.""TicketCode"",
                    t.""Status""
                FROM ""Tickets"" t
                INNER JOIN ""Bookings"" b ON t.""BookingId"" = b.""Id""
                INNER JOIN ""Events"" e ON t.""EventId"" = e.""Id""
                WHERE b.""CustomerId"" = @CustomerId 
                  AND b.""Status"" = 1"; 

            var tickets = await connection.QueryAsync<PurchasedTicketDto>(sql, new { CustomerId = request.CustomerId });

            return tickets;
        }
    }
}