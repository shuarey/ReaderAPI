using System.Data;
using Microsoft.Data.SqlClient;

namespace ReaderAPI.Infrastructure
{
    public interface IDatabaseConnection
    {
        IDbConnection GetConnection ( );
    }

    public class DBConnectionService : IDatabaseConnection
    {
        private readonly IDbConnection _connection;

        public DBConnectionService ( IConfiguration configuration )
        {
            _connection = new SqlConnection ( configuration.GetConnectionString ( "DefaultConnection" ) );
        }

        public IDbConnection GetConnection ( )
        {
            return _connection;
        }
    }
}
