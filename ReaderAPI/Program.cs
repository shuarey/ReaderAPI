using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ReaderAPI.Infrastructure;
using ReaderAPI.Middleware;
using ReaderAPI.Services;

var builder = WebApplication.CreateBuilder ( args );
Console.WriteLine ( "Starting application..." );

// Add controllers
builder.Services.AddControllers ( );
Console.WriteLine ( "Added controllers." );

// Add API explorer for endpoints
builder.Services.AddEndpointsApiExplorer ( );
Console.WriteLine ( "Added endpoints API explorer." );

// Add Swagger for API documentation
builder.Services.AddSwaggerGen ( );
Console.WriteLine ( "Added Swagger for API documentation." );

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor ( );
Console.WriteLine ( "Added HttpContextAccessor." );

// Configure CORS
builder.Services.AddCors ( options =>
{
    Console.WriteLine ( "Configuring CORS policy..." );
    options.AddPolicy ( "AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin ( )
               .AllowAnyMethod ( )
               .AllowAnyHeader ( );
    } );
    Console.WriteLine ( "CORS policy 'AllowAllOrigins' configured." );
} );

// Configure Serilog
Log.Logger = new LoggerConfiguration ( )
    .ReadFrom.Configuration ( builder.Configuration )
    .Enrich.FromLogContext ( )
    .WriteTo.Console ( )
    //.WriteTo.Map (
    //    "FilePath",
    //    "default",
    //    ( filePath, wt ) =>
    //    {
    //        wt.File (
    //            path: $"C:/Reader/Logs{filePath}/.log",
    //            rollingInterval: RollingInterval.Hour,
    //            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    //        );
    //    }
    //)
    .CreateLogger ( );
Console.WriteLine ( "Configured Serilog logging." );

// Use Serilog as the logging provider
builder.Host.UseSerilog ( );
Console.WriteLine ( "Serilog set as the logging provider." );

// Configure API behavior
builder.Services.Configure<ApiBehaviorOptions> ( options =>
{
    options.SuppressModelStateInvalidFilter = true;
} );
Console.WriteLine ( "Configured API behavior to suppress model state invalid filter." );

// Register services
builder.Services.AddSingleton<ITypeResolver, TypeResolver> ( );
Console.WriteLine ( "Registered ITypeResolver as a singleton." );

builder.Services.AddScoped<IDatabaseConnection, DBConnectionService> ( );
Console.WriteLine ( "Registered IDatabaseConnection as a scoped service." );

builder.Services.AddScoped<AccountUserService> ( );
Console.WriteLine ( "Registered AccountUserService as a scoped service." );

// Build the application
var app = builder.Build ( );
Console.WriteLine ( "Application built successfully." );

// Configure middleware for development environment
if ( app.Environment.IsDevelopment ( ) )
{
    Console.WriteLine ( "Configuring development environment..." );
    app.UseSwagger ( );
    Console.WriteLine ( "Swagger enabled." );

    app.UseSwaggerUI ( );
    Console.WriteLine ( "Swagger UI enabled." );

    app.UseForwardedHeaders ( new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor,
        ForwardLimit = 1 // Only trust the first forwarded header
    } );
    Console.WriteLine ( "Forwarded headers configured." );
}

// Configure CORS middleware
app.UseCors ( "AllowAllOrigins" );
Console.WriteLine ( "CORS middleware applied." );

// Add custom logging middleware
app.UseMiddleware<Logging> ( );
Console.WriteLine ( "Custom logging middleware added." );

// Enable HTTPS redirection
app.UseHttpsRedirection ( );
Console.WriteLine ( "HTTPS redirection enabled." );

// Enable authorization
app.UseAuthorization ( );
Console.WriteLine ( "Authorization middleware applied." );

// Map controllers
app.MapControllers ( );
Console.WriteLine ( "Controllers mapped." );

// Run the application
app.Run ( );
Console.WriteLine ( "Application is running." );
