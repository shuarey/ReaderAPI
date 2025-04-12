using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ReaderAPI.Models
{
    public class BaseClasses
    {
        public class BaseResponse : IActionResult
        {
            public bool success { get; set; }
            public string message { get; set; }

            public async Task ExecuteResultAsync ( ActionContext context )
            {
                var oResult = new ObjectResult ( this )
                {
                    StatusCode = this is BasicErrorResponse errorResponse ? ( int ) errorResponse.status : StatusCodes.Status200OK
                };
                await oResult.ExecuteResultAsync ( context );
            }
        }

        public class BasicErrorResponse : BaseResponse
        {
            public BasicErrorResponse ( string errorMessage, HttpStatusCode status )
            {
                base.success = false;
                base.message = errorMessage;
                this.status = status;
            }
            public HttpStatusCode status { get; set; }
        }
    }
}
