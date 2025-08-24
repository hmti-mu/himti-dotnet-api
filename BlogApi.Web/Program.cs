using BlogApi.Application.Interfaces;
using BlogApi.Application.UseCases;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Data;
using BlogApi.Infrastructure.Repositories;
using BlogApi.Infrastructure.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<BlogDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
}
else
{
    builder.Services.AddDbContext<BlogDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
}

// Register Repositories
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();

// Register Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Add Authentication with JWT Bearer
var jwtKey = builder.Configuration["JwtSettings:SecretKey"] ?? "YourSuperSecretJwtKeyThatIsAtLeast256BitsLong!";
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "BlogApi";
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "BlogApiUsers";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

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
builder.Services.AddScoped<UploadMediaUseCase>();
builder.Services.AddScoped<GetMediaUseCase>();
builder.Services.AddScoped<DeleteMediaUseCase>();

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

// Configure static file serving for uploaded media
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();