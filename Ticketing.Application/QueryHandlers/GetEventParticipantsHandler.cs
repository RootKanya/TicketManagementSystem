using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Ticketing.Application.DTOs;
using Ticketing.Application.ExternalInterfaces;
using Ticketing.Application.Queries;

namespace Ticketing.Application.QueryHandlers;

public class GetEventParticipantsHandler : IRequestHandler<GetEventParticipantsQuery, IEnumerable<ParticipantDto>>
{
    private readonly IQueryConnectionFactory _connectionFactory;

    public GetEventParticipantsHandler(IQueryConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ParticipantDto>> Handle(GetEventParticipantsQuery query, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
            SELECT 
                c.Name AS CustomerName, 
                tc.Name AS TicketCategory, 
                t.Code AS TicketCode, 
                t.Status AS CheckInStatus
            FROM Bookings b
            JOIN Customers c ON b.CustomerId = c.Id
            JOIN Tickets t ON b.Id = t.BookingId
            JOIN TicketCategories tc ON b.TicketCategoryId = tc.Id
            WHERE b.EventId = @EventId AND b.Status = 'Paid'";

        return await connection.QueryAsync<ParticipantDto>(sql, new { query.EventId });
    }
}