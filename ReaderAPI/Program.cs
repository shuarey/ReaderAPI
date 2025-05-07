using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ReaderAPI.Infrastructure;
using ReaderAPI.Middleware;
using ReaderAPI.Services;
using ReaderAPI.Utilities;

var builder = WebApplication.CreateBuilder ( args );
builder.Services.AddControllers ( );
builder.Services.AddEndpointsApiExplorer ( );
builder.Services.AddSwaggerGen ( );
builder.Services.AddHttpContextAccessor ( );

builder.Services.AddCors ( options =>
{
    options.AddPolicy ( "AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin ( )
               .AllowAnyMethod ( )
               .AllowAnyHeader ( );
    } );
} );

Log.Logger = new LoggerConfiguration ( )
    .ReadFrom.Configuration ( builder.Configuration )
    .Enrich.FromLogContext ( )
    .WriteTo.Console ( )
    .WriteTo.Map (
        "FilePath",
        "default",
        ( filePath, wt ) =>
        {
            wt.File (
                path: $"C:/Reader/Logs{filePath}/.log",
                rollingInterval: RollingInterval.Hour,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            );
        }
    )
    .CreateLogger ( );

builder.Host.UseSerilog ( );
builder.Services.Configure<ApiBehaviorOptions> ( options =>
{
    options.SuppressModelStateInvalidFilter = true;
} );

builder.Services.AddSingleton<ITypeResolver, TypeResolver> ( );
builder.Services.AddScoped<IDatabaseConnection, DBConnectionService> ( );
builder.Services.AddScoped<AccountUserService> ( );

var app = builder.Build ( );

if ( app.Environment.IsDevelopment ( ) )
{
    app.UseSwagger ( );

    app.UseSwaggerUI ( );

    app.UseForwardedHeaders ( new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor,
        ForwardLimit = 1 // Only trust the first forwarded header
    } );
}

app.UseCors ( "AllowAllOrigins" );
app.UseMiddleware<Logging> ( );
app.UseHttpsRedirection ( );
app.UseAuthorization ( );
app.MapControllers ( );

app.Run ( );