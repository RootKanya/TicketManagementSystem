using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;

namespace Ticketing.Application.QueryHandlers;

using global::Ticketing.Application.DTOs;
using global::Ticketing.Application.ExternalInterfaces;
using global::Ticketing.Application.Queries;

public class GetEventDetailsHandler
{
    private readonly IQueryConnectionFactory _connectionFactory;

    public GetEventDetailsHandler(IQueryConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<EventDetailDto?> Handle(GetEventDetailsQuery query, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        string eventSql = @"
            SELECT Id, Name, Description, StartDate AS Date, Location 
            FROM Events WHERE Id = @EventId";

        var eventDetail = await connection.QuerySingleOrDefaultAsync<EventDetailDto>(eventSql, new { query.EventId });

        if (eventDetail == null) return null;

        string categorySql = @"
            SELECT Id, Name, PriceAmount AS Price, Quota, ReservedQuantity, SalesStartDate, SalesEndDate 
            FROM TicketCategories 
            WHERE EventId = @EventId AND IsActive = true";

        var rawCategories = await connection.QueryAsync<dynamic>(categorySql, new { query.EventId });
        var categories = new List<TicketCategoryDto>();
        var now = DateTime.UtcNow;

        foreach (var row in rawCategories)
        {
            string status = "Active";
            int remainingQuota = row.Quota - row.ReservedQuantity;

            if (now < row.SalesStartDate)
                status = "Coming Soon";
            else if (now > row.SalesEndDate)
                status = "Sales Closed";
            else if (remainingQuota <= 0)
                status = "Sold Out";

            categories.Add(new TicketCategoryDto(row.Id, row.Name, row.Price, status));
        }

        return eventDetail with { Categories = categories };
    }
}
