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

public class GetEventSalesReportQueryHandler
{
    private readonly IQueryConnectionFactory _connectionFactory;

    public GetEventSalesReportQueryHandler(IQueryConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<SalesReportDto> Handle(GetEventSalesReportQuery query, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        string bookingStatsSql = @"
            SELECT 
                COUNT(CASE WHEN Status = 0 THEN 1 END) AS PendingBookings,
                COUNT(CASE WHEN Status = 1 THEN 1 END) AS PaidBookings,
                COUNT(CASE WHEN Status = 2 THEN 1 END) AS ExpiredBookings,
                COUNT(CASE WHEN Status = 3 THEN 1 END) AS RefundedBookings,
                COALESCE(SUM(CASE WHEN Status = 1 THEN TotalPriceAmount ELSE 0 END), 0) AS TotalRevenue
            FROM Bookings
            WHERE EventId = @EventId";

        var stats = await connection.QuerySingleAsync<dynamic>(bookingStatsSql, new { query.EventId });

        string categoryStatsSql = @"
            SELECT 
                tc.Name AS CategoryName, 
                COALESCE(SUM(b.QuantityValue), 0) AS TicketsSold
            FROM Bookings b
            JOIN TicketCategories tc ON b.TicketCategoryId = tc.Id
            WHERE b.EventId = @EventId AND b.Status = 1
            GROUP BY tc.Name";

        var categoryStatsRaw = await connection.QueryAsync<dynamic>(categoryStatsSql, new { query.EventId });
        var ticketsSoldPerCategory = categoryStatsRaw.ToDictionary(
            row => (string)row.CategoryName,
            row => (int)row.TicketsSold
        );

        return new SalesReportDto(
            ticketsSoldPerCategory,
            (int)stats.PendingBookings,
            (int)stats.PaidBookings,
            (int)stats.ExpiredBookings,
            (int)stats.RefundedBookings,
            (decimal)stats.TotalRevenue
        );
    }
}
