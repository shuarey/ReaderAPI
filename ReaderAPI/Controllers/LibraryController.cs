using Microsoft.AspNetCore.Mvc;
using ReaderAPI.Services;

namespace ReaderAPI.Controllers
{
    [ApiController]
    [Route ( "[controller]" )]
    public class LibraryController : ControllerBase
    {
        private readonly LibraryService _LibraryService;

        public LibraryController ( LibraryService libraryService )
        {
            _LibraryService = libraryService;
        }

        [HttpGet]
        public IActionResult GetLibrary ( string accountUserID )
        {
            return _LibraryService.GetLibrary ( accountUserID );
        }

        [HttpPost ( "add" )]
        public IActionResult AddBook ( [FromBody] string bookID )
        {
            return _LibraryService.AddBook ( bookID );
        }
    }
}
