using ReaderAPI.Utilities;

namespace ReaderAPI.Middleware
{
    public class Logging
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Logging> _logger;

        public Logging ( RequestDelegate next, ILogger<Logging> logger )
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke ( HttpContext context )
        {
            context.Request.EnableBuffering ( );
            using var reader = new StreamReader ( context.Request.Body, leaveOpen: true );
            var requestBody = await reader.ReadToEndAsync ( );
            context.Request.Body.Position = 0;

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream ( );
            context.Response.Body = responseBody;

            await _next ( context );

            // Skip logging for HTTP 307 responses. Still working on getting the log to skip all logging, even the 307 redirect. It's still showing the timestamp even though it's not doing the details
            if ( context.Response.StatusCode == StatusCodes.Status307TemporaryRedirect )
            {
                await responseBody.CopyToAsync ( originalBodyStream );
                return;
            }

            // Read and log response body
            context.Response.Body.Seek ( 0, SeekOrigin.Begin );
            var responseText = await new StreamReader ( context.Response.Body ).ReadToEndAsync ( );
            context.Response.Body.Seek ( 0, SeekOrigin.Begin );

            _logger.LogInformation ( "\nHTTP {Method} {Path} responded {StatusCode}\nRequest: {RequestBody}\nResponse: {ResponseBody}\n",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                requestBody,
                Utility.FormatJson ( responseText ) );

            await responseBody.CopyToAsync ( originalBodyStream );
        }
    }
}
