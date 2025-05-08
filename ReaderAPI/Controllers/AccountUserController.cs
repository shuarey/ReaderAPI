using Microsoft.AspNetCore.Mvc;
using ReaderAPI.Services;
using static ReaderAPI.Models.RequestClasses;

namespace ReaderAPI.Controllers
{
    [ApiController]
    [Route ( "[controller]" )]
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

        [HttpGet]
        public IActionResult GetAccountUser ( [FromQuery] string user_id )
        {
            return _AccountUserService.GetAccountUser ( user_id );
        }

        [HttpPut]
        public IActionResult UpdateAccountUser ( [FromBody] AccountUserPUTRequest request )
        {
            return _AccountUserService.UpdateAccountUser ( request );
        }
    }
}
