using System.Net;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using ReaderAPI.Infrastructure;
using ReaderAPI.Models;
using static ReaderAPI.Models.BaseClasses;

namespace ReaderAPI.Services
{
    public class LibraryService : BaseService
    {
        public LibraryService ( IHttpContextAccessor context, IDatabaseConnection connection, ILogger<AccountUserService> logger ) : base ( context, connection, logger ) { }

        public BaseResponse GetLibrary ( string accountUserID )
        {
            return null;
        }

        internal IActionResult AddBook ( string bookID )
        {
            throw new NotImplementedException ( );
        }
    }
}