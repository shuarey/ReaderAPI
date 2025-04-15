# ReaderAPI

ReaderAPI is a .NET 8-based API designed to handle user account management and logging functionality. This guide will help you set up and run the project locally.

---

## Notes

Logging is a WIP. Currently the json configuration defines two routes for logging, but at the moment, all logs are written in both because the Enricher is not being defined properly.

## Prerequisites

Before you begin, ensure you have the following installed on your system:

1. **.NET 8 SDK**  
   Download and install the .NET 8 SDK from [Microsoft's official site](https://dotnet.microsoft.com/).

2. **SQL Server**  
   Ensure you have a running instance of SQL Server for database connectivity.

3. **Git**  
   Install Git to clone the repository. You can download it from [git-scm.com](https://git-scm.com/).

4. **Visual Studio 2022**  
   Install Visual Studio 2022 with the following workloads:
   - ASP.NET and web development
   - .NET Core cross-platform development

---

## Getting Started

Follow these steps to set up and run the project locally:

### 1. Clone the Repository
Clone the repository to your local machine:
git clone https://github.com/shuarey/ReaderAPI.git cd ReaderAPI

### 2. Configure the Database
- Update the database connection string in the `appsettings.json` file located in the root of the project. Example:
- Replace `YOUR_SERVER_NAME` with your SQL Server instance name.
"ConnectionStrings": { "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ReaderAPI;Trusted_Connection=True;" }

### 3. Restore Dependencies
Restore the required NuGet packages:
dotnet restore

### 4. Build the Project
Build the project to ensure everything is set up correctly:
dotnet build

### 5. Run the Application
Start the application:

The API will be available at `https://localhost:5001` or `http://localhost:5000`.

---

## Features

- **User Account Management**: Includes endpoints for user registration, login, and account management.
- **Custom Middleware**: Logs HTTP requests and responses, excluding HTTP 307 redirects.
- **Swagger Integration**: API documentation is available at `/swagger` when running in development mode.

---

## Development Notes

- **CORS Policy**: The project currently allows all origins, methods, and headers for development purposes. Update the CORS policy in `Program.cs` for production.
- **Logging**: The project uses Serilog for structured logging. Configure logging settings in `appsettings.json`.

---

## Troubleshooting

1. **Port Conflicts**: If the default ports (`5000`/`5001`) are in use, update the `launchSettings.json` file in the `Properties` folder.
2. **Database Connection Issues**: Ensure your SQL Server instance is running and the connection string is correct.
3. **Missing Dependencies**: Run `dotnet restore` to ensure all dependencies are installed.

---

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request for any changes.