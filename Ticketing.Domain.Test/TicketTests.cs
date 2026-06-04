using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Tests;

public class TicketTests
{
    [Fact]
    public void CheckIn_Should_ThrowException_When_AlreadyCheckedIn()
    {
        var ticket = new Ticket(Guid.NewGuid(), Guid.NewGuid(), "EVT-2026-XYZ");
        ticket.CheckIn();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            ticket.CheckIn());

        Assert.Equal("Ticket is already marked CheckedIn.", exception.Message);
    }
}