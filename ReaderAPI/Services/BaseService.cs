using System.Data;
using ReaderAPI.Infrastructure;

namespace ReaderAPI.Services
{
    public abstract class BaseService : IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly IHttpContextAccessor _context;
        private readonly ILogger _logger;

        public BaseService ( IHttpContextAccessor context, IDatabaseConnection dbConnexService, ILogger logger )
        {
            _connection = dbConnexService.GetConnection ( );
            _context = context;
            _logger = logger;
        }

        protected IDbConnection Connection => _connection;
        protected IHttpContextAccessor Context => _context;

        protected void LogInfo ( ) => _logger.LogInformation ( null );
        protected void LogError ( Exception ex = null, string message = "" ) => _logger.LogError ( ex, message );

        public void Dispose ( )
        {
            _connection.Dispose ( );
        }
    }
}
