using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;
using Ticketing.Application.ExternalInterfaces;

namespace Ticketing.Infrastructure.Services
{
    public class NpgsqlConnectionFactory : IQueryConnectionFactory
    {
        private readonly string _connectionString;

        public NpgsqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Database connection string is missing in appsettings.json.");
        }

        public IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}