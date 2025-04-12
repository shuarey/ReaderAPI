using Microsoft.AspNetCore.Mvc;
using ReaderAPI.Models;
using ReaderAPI.Services;

namespace ReaderAPI.Controllers
{
    [ApiController]
    [Route ( "api/[controller]" )]
    public class AccountUserController : ControllerBase
    {
        private readonly AccountUserService _AccountUserService;

        public AccountUserController ( AccountUserService accountUserService )
        {
            _AccountUserService = accountUserService;
        }
        
        [HttpPost ( "login" )]
        public IActionResult Login ( [FromBody] AccountUserLoginPOSTRequest request )
        {
            return _AccountUserService.LoginUser ( request );
        }

        [HttpPost ( "register" )]
        public IActionResult Register ( [FromBody] AccountUserRegisterPOSTRequest request )
        {
            return _AccountUserService.RegisterAccountUser ( request );
        }
    }
}
