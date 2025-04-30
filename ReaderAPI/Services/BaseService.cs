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

        protected IDbConnection DBConnection => _connection;
        protected IHttpContextAccessor Context => _context;

        public void Dispose ( )
        {
            _connection.Dispose ( );
        }
    }
}
