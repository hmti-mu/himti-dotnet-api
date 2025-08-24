# HIMTI .NET API

Blog API is a .NET 8.0 Web API built using Clean Architecture principles. It provides RESTful endpoints for managing blog articles with Entity Framework Core for data persistence and Swagger/OpenAPI for documentation.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Prerequisites and Setup
- Install .NET 8.0 SDK (version 8.0.119 or later)
- Install Entity Framework tools: `dotnet tool install --global dotnet-ef`

### Bootstrap, Build, and Test
- Restore packages: `dotnet restore` -- takes 10-12 seconds. NEVER CANCEL. Set timeout to 30+ seconds.
- Build solution: `dotnet build` -- takes 10-11 seconds. NEVER CANCEL. Set timeout to 30+ seconds.
- Apply database migrations: `dotnet ef database update --project BlogApi.Infrastructure --startup-project BlogApi.Web` -- takes 5-10 seconds.

### Run the Application
- ALWAYS run the bootstrapping steps first.
- Start the API: `cd BlogApi.Web && dotnet run` -- starts immediately, NEVER CANCEL.
- The application will be available at: http://localhost:5150
- Swagger UI is available at: http://localhost:5150/swagger

## Validation

### Manual Testing Requirements
- Always test the API endpoints after making changes:
  - GET http://localhost:5150/api/articles (should return JSON array)
  - Swagger UI should be accessible at http://localhost:5150/swagger/index.html
- ALWAYS run through at least one complete end-to-end scenario after making changes.
- Test the database connection by calling the articles endpoint and verifying it returns an empty array `[]` on fresh setup.

### Build Validation
- The build succeeds with some nullable reference warnings (expected behavior).
- Build warnings about nullable properties in Article.cs and ArticleDto.cs are known and acceptable.
- Always run `dotnet build` before making commits to ensure compilation succeeds.

## Architecture and Code Organization

### Project Structure
```
BlogApi.sln                    # Main solution file
├── BlogApi.Domain/            # Domain entities and interfaces
│   ├── Article.cs            # Article entity (Note: Content is int type)
│   └── Interfaces/IArticleRepository.cs
├── BlogApi.Application/       # Use cases and DTOs
│   ├── DTOs/ArticlesDto.cs   # Data transfer objects
│   └── UseCases/GetArticleUseCase.cs
├── BlogApi.Infrastructure/    # Data access layer
│   ├── Data/BlogDbContext.cs # Entity Framework context
│   ├── Repositories/ArticleRepository.cs
│   └── Migrations/           # EF Core migrations
└── BlogApi.Web/              # ASP.NET Core Web API
    ├── Program.cs            # Application startup and configuration
    ├── Properties/launchSettings.json
    └── appsettings.Development.json
```

### Key Implementation Details
- **Database**: SQLite with Entity Framework Core (configured for local development)
- **Clean Architecture**: Proper separation of concerns across layers
- **Dependency Injection**: Configured in Program.cs
- **API Documentation**: Swagger/OpenAPI integrated
- **CORS**: Enabled for development (AllowAll policy)

### Important Files to Check
- Always check `Program.cs` when modifying dependency injection or middleware configuration
- Review `BlogDbContext.cs` when working with data models
- Check migration files when database schema changes are needed

## Database Management

### Local Development Database
- Uses SQLite database (`blogapi.db`) for portability
- Database is created automatically when running migrations
- Connection string configured in `appsettings.Development.json`

### Common Database Tasks
- Create new migration: `dotnet ef migrations add <MigrationName> --project BlogApi.Infrastructure --startup-project BlogApi.Web`
- Apply migrations: `dotnet ef database update --project BlogApi.Infrastructure --startup-project BlogApi.Web`
- Remove last migration: `dotnet ef migrations remove --project BlogApi.Infrastructure --startup-project BlogApi.Web`

### Database Schema Notes
- Article entity has unusual Content field type (int instead of string)
- This appears to be intentional based on existing migrations and DTO mapping
- Always verify Content field handling when making changes to Article-related code

## API Endpoints

### Currently Available
- GET `/api/articles` - Returns all articles as JSON array
- Swagger documentation available at `/swagger`

### Development Features
- Swagger UI enabled in Development environment
- CORS configured to allow all origins in development
- Detailed error pages in Development environment

## Common Issues and Solutions

### Database Issues
- If migration fails, ensure the BlogApi.Web project is set as startup project
- SQLite database files (*.db, *.db-shm, *.db-wal) are ignored by git
- For SQL Server setup, update connection string and change `UseSqlite` to `UseSqlServer` in Program.cs

### Build Issues
- Nullable reference warnings are expected and can be ignored
- If restore fails, try `dotnet clean` followed by `dotnet restore`
- Ensure .NET 8.0 SDK is installed

### Runtime Issues
- Port 5150 must be available for HTTP
- Port 7223 must be available for HTTPS (if using HTTPS profile)
- Check that Entity Framework tools are installed globally

## Development Workflow

1. Make code changes
2. Run `dotnet build` to verify compilation
3. Apply any new migrations with `dotnet ef database update`
4. Test the application with `dotnet run` in BlogApi.Web folder
5. Validate endpoints work correctly via curl or Swagger UI
6. Always test the GET /api/articles endpoint as a basic health check

## Performance Expectations

- **Package Restore**: 10-12 seconds
- **Build**: 10-11 seconds  
- **Migration Apply**: 5-10 seconds
- **Application Startup**: 2-3 seconds
- All timings are for typical development machine; allow 50% buffer for timeouts

NEVER CANCEL any of these operations as they may be performing important work even if they appear to hang.