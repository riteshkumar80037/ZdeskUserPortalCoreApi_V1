using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ZdeskUserPortal.DataAccess
{
    public sealed class SqlDbConnectionFactory : IDbConnectionFactory
    {
        public static IConfiguration StaticConfiguration { get; set; } = null;

        private IConfiguration Configuration { get; } = null;

        internal SqlDbConnectionFactory()
        {
            Configuration = StaticConfiguration;
        }

        public SqlDbConnectionFactory(IConfiguration config)
        {
            Configuration = config;
        }

        public IDbConnection CreateDbConnection(string connectionName)
        {
            var connectionString = Configuration?.GetConnectionString(connectionName);

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                var connection = new SqlConnection(connectionString);
                if (connection is not null)
                {
                    return connection;
                }
            }

            throw new ArgumentNullException($"Failed to create a connection for connection named '{connectionName}'.");
        }
    }
}
