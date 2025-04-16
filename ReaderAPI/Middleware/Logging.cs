using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using ReaderAPI.Services;
using ReaderAPI.Utilities;
using Serilog.Context;

namespace ReaderAPI.Middleware
{
    public class Logging ( RequestDelegate next, ILogger<Logging> logger, ITypeResolver typeResolver )
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<Logging> _logger = logger;
        private readonly ITypeResolver _typeResolver = typeResolver;

        public async Task Invoke ( HttpContext context )
        {
            var path = context.Request.Path.ToString ( );
            using ( LogContext.PushProperty ( "FilePath", path ) )
            {
                var stopWatch = Stopwatch.StartNew ( );

                context.Request.EnableBuffering ( );
                using var reader = new StreamReader ( context.Request.Body, leaveOpen: true );
                var requestBody = await reader.ReadToEndAsync ( );
                context.Request.Body.Position = 0;

                Type requestType = _typeResolver.GetRequestType ( context.Request.Path );
                object deserializedRequest = null;

                if ( requestType != null )
                    deserializedRequest = JsonConvert.DeserializeObject ( requestBody, requestType, new JsonSerializerSettings { Formatting = Formatting.Indented } );

                var originalBodyStream = context.Response.Body;
                using var responseBody = new MemoryStream ( );
                context.Response.Body = responseBody;

                await _next ( context );

                // Skip logging for HTTP 307 responses
                if ( context.Response.StatusCode == StatusCodes.Status307TemporaryRedirect )
                {
                    await responseBody.CopyToAsync ( originalBodyStream );
                    return;
                }

                context.Response.Body.Seek ( 0, SeekOrigin.Begin );
                var responseText = await new StreamReader ( context.Response.Body ).ReadToEndAsync ( );
                context.Response.Body.Seek ( 0, SeekOrigin.Begin );

                var sanitizedRequestBody = deserializedRequest != null
                    ? JsonConvert.SerializeObject ( deserializedRequest, requestType, new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                    } )
                    : requestBody;

                var sanitizedResponseBody = Utility.FormatJson ( responseText );

                stopWatch.Stop ( );
                var elapsedMs = stopWatch.ElapsedMilliseconds;

                _logger.LogInformation (
                    "\n{Protocol} {Method} {Path} in {ElapsedTime}ms\nHost: {Host}\nAccept: {Accept}\nAccept-Encoding: {AcceptEncoding}\nContent-Type: {ContentType}\nContent-Length: {ContentLength}\nRequest body:\n{RequestBody}\n\n{Protocol} Response status: {ResponseStatus} {ResponseCode}\nContent-Type: {ResponseContentType}\nResponse body: \n{ResponseBody}\n",
                    context.Request.Protocol,
                    context.Request.Method,
                    context.Request.Path,
                    elapsedMs,
                    context.Request.Host,
                    context.Request.Headers [ "Accept" ].ToString ( ) ?? null,
                    context.Request.Headers [ "Accept-Encoding" ].ToString ( ) ?? null,
                    context.Request.Headers [ "Content-Type" ].ToString ( ) ?? null,
                    context.Request.Headers [ "Content-Length" ].ToString ( ) ?? null,
                    sanitizedRequestBody,
                    context.Request.Protocol,
                    context.Response.StatusCode,
                    ( HttpStatusCode ) context.Response.StatusCode,
                    context.Response.Headers [ "Content-Type" ].ToString ( ) ?? null,
                    sanitizedResponseBody );

                await responseBody.CopyToAsync ( originalBodyStream );
            }
        }
    }

    public enum LoggingCategory
    {
        AccountUserLogin,
        AccountUserRegister
    }
}