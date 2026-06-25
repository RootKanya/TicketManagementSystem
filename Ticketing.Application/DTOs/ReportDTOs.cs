using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.DTOs;

public record SalesReportDto(
    Dictionary<string, int> TicketsSoldPerCategory,
    int PendingBookings,
    int PaidBookings,
    int ExpiredBookings,
    int RefundedBookings,
    decimal TotalRevenue);

public record ParticipantDto(string CustomerName, string TicketCategory, string TicketCode, string CheckInStatus);
