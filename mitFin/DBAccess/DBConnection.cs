using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;

namespace MitFin_Api.DBAccess
{
    // Handles Oracle DB connection creation
    public class DBConnection
    {
        private readonly string _connectionString;

        public DBConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultOracle")!;
        }


        public OracleConnection CreateConnection()
        {
            return new OracleConnection(_connectionString);
        }
    }
}
