using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ReaderAPI.Infrastructure;
using ReaderAPI.Middleware;
using ReaderAPI.Services;

var builder = WebApplication.CreateBuilder ( args );

builder.Services.AddControllers ( );
builder.Services.AddEndpointsApiExplorer ( );
builder.Services.AddSwaggerGen ( );
builder.Services.AddHttpContextAccessor ( );
builder.Services.AddCors ( options =>
{
    // using this just for dev purposes. Will eventually only accept calls from verified servers
    options.AddPolicy ( "AllowAllOrigins",
        builder =>
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
        ( filePath, wt ) => {
            wt.File (
                path: $"C:/Reader/Logs{filePath}/.log",
                rollingInterval: RollingInterval.Hour,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            );
        }
    )
    .CreateLogger ( );

builder.Host.UseSerilog ( );

// disabling default field validation. Endpoints will handle field validation on an individual level
// TODO: Build middleware to handle custom error response.
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

// This is just for development purposes. Will eventually only allow calls from hosting server
app.UseCors ( "AllowAllOrigins" );
app.UseMiddleware<Logging> ( );
app.UseHttpsRedirection ( );

app.UseAuthorization ( );
app.MapControllers ( );

app.Run ( );
