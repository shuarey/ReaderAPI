using System.Data;
using ReaderAPI.Infrastructure;
using ReaderAPI.Middleware;

namespace ReaderAPI.Services
{
    public abstract class BaseService : IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly IHttpContextAccessor _context;
        private readonly ILogger _logger;

        public BaseService ( IHttpContextAccessor context, IDatabaseConnection dbConnexService, ILogger logger, ILoggerFactory loggerFactory )
        {
            _connection = dbConnexService.GetConnection ( );
            _context = context;
            _logger = logger;
        }

        protected IDbConnection Connection => _connection;
        protected IHttpContextAccessor Context => _context;

        public void Dispose ( )
        {
            _connection.Dispose ( );
        }
    }
}
