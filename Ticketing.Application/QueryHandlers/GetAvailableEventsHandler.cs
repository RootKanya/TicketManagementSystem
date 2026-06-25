using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Ticketing.Application.DTOs;
using Ticketing.Application.ExternalInterfaces;
using Ticketing.Application.Queries;

namespace Ticketing.Application.QueryHandlers;

public class GetAvailableEventsHandler : IRequestHandler<GetAvailableEventsQuery, IEnumerable<EventListItemDto>>
{
    private readonly IQueryConnectionFactory _connectionFactory;

    public GetAvailableEventsHandler(IQueryConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<EventListItemDto>> Handle(GetAvailableEventsQuery query, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
            SELECT 
                e.Id, 
                e.Name, 
                e.StartDate AS Date, 
                e.Location,
                COALESCE(MIN(tc.PriceAmount), 0) AS LowestPrice
            FROM Events e
            LEFT JOIN TicketCategories tc ON e.Id = tc.EventId AND tc.IsActive = true
            WHERE e.Status = 1 
            AND (@FilterDate IS NULL OR DATE(e.StartDate) = DATE(@FilterDate))
            AND (@FilterLocation IS NULL OR e.Location ILIKE '%' || @FilterLocation || '%')
            GROUP BY e.Id, e.Name, e.StartDate, e.Location;";

        return await connection.QueryAsync<EventListItemDto>(sql, new
        {
            query.FilterDate,
            FilterLocation = query.FilterLocation
        });
    }
}