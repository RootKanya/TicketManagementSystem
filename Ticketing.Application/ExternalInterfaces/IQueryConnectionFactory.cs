using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Ticketing.Application.ExternalInterfaces;

public interface IQueryConnectionFactory
{
    IDbConnection CreateConnection();
}
