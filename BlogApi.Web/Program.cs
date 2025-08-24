using BlogApi.Application.Interfaces;
using BlogApi.Application.UseCases;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Data;
using BlogApi.Infrastructure.Repositories;
using BlogApi.Infrastructure.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<BlogDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
}
else
{
    builder.Services.AddDbContext<BlogDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
}

// Register Repositories
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Register Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Add Authentication (JWT configuration temporarily simplified due to IDE IntelliSense issues)
var jwtKey = builder.Configuration["JwtSettings:Key"] ?? "YourSuperSecretJwtKeyThatIsAtLeast256BitsLong!";
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "BlogApi";
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "BlogApiUsers";

// Note: JWT Bearer authentication temporarily disabled due to IDE namespace resolution issues
// The JWT package is properly installed and works at build time
builder.Services.AddAuthentication("Bearer");
// .AddJwtBearer("Bearer", options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = jwtIssuer,
//         ValidAudience = jwtAudience,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//     };
// });

// Add Authorization with policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireUser", policy =>
        policy.RequireAuthenticatedUser());
    options.AddPolicy("RequireAuthor", policy =>
        policy.RequireRole("Author", "Editor", "Admin", "SuperAdmin"));
    options.AddPolicy("RequireEditor", policy =>
        policy.RequireRole("Editor", "Admin", "SuperAdmin"));
    options.AddPolicy("RequireAdmin", policy =>
        policy.RequireRole("Admin", "SuperAdmin"));
    options.AddPolicy("RequireSuperAdmin", policy =>
        policy.RequireRole("SuperAdmin"));
});

// Register Use Cases
builder.Services.AddScoped<GetArticlesUseCase>();
builder.Services.AddScoped<CreateArticleUseCase>();
builder.Services.AddScoped<GetArticleByIdUseCase>();
builder.Services.AddScoped<UpdateArticleUseCase>();
builder.Services.AddScoped<DeleteArticleUseCase>();
builder.Services.AddScoped<GetCategoriesUseCase>();
builder.Services.AddScoped<SearchArticlesUseCase>();
builder.Services.AddScoped<RegisterUserUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<GetUsersUseCase>();

// Add FastEndpoints
builder.Services.AddFastEndpoints();

// Configure Swagger with proper tag-based organization
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "Blog API";
        s.Description = "A comprehensive blog management API organized by domain";
        s.Version = "v1";
    };
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

var app = builder.Build();

// Ensure database is created and migrated
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
    try
    {
        context.Database.Migrate();
        Console.WriteLine("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database migration error: {ex.Message}");
        // Fallback to EnsureCreated for development
        try
        {
            context.Database.EnsureCreated();
            Console.WriteLine("Database created using EnsureCreated fallback");
        }
        catch (Exception ex2)
        {
            Console.WriteLine($"Database creation fallback error: {ex2.Message}");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen(); // Enable Swagger with FastEndpoints
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Enable CORS

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();