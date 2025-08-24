using BlogApi.Application.Interfaces;
using BlogApi.Application.UseCases;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Data;
using BlogApi.Infrastructure.Repositories;
using BlogApi.Infrastructure.Services;
using BlogApi.Web.Authorization;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
}

// Register Repositories
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Register Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Register Use Cases
builder.Services.AddScoped<GetArticlesUseCase>();
builder.Services.AddScoped<CreateArticleUseCase>();
builder.Services.AddScoped<GetArticleByIdUseCase>();
builder.Services.AddScoped<UpdateArticleUseCase>();
builder.Services.AddScoped<DeleteArticleUseCase>();
builder.Services.AddScoped<RegisterUserUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<GetUsersUseCase>();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "BlogApiDefaultSecretKeyForDevelopmentOnly123456789";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"] ?? "BlogApi",
            ValidAudience = jwtSettings["Audience"] ?? "BlogApiUsers",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// Configure Authorization Policies
builder.Services.AddSingleton<IAuthorizationHandler, MinimumRoleLevelHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyNames.RequireUser, policy =>
        policy.Requirements.Add(new MinimumRoleLevelRequirement(RoleLevels.User)));
    
    options.AddPolicy(PolicyNames.RequireAuthor, policy =>
        policy.Requirements.Add(new MinimumRoleLevelRequirement(RoleLevels.Author)));
    
    options.AddPolicy(PolicyNames.RequireEditor, policy =>
        policy.Requirements.Add(new MinimumRoleLevelRequirement(RoleLevels.Editor)));
    
    options.AddPolicy(PolicyNames.RequireAdmin, policy =>
        policy.Requirements.Add(new MinimumRoleLevelRequirement(RoleLevels.Admin)));
    
    options.AddPolicy(PolicyNames.RequireSuperAdmin, policy =>
        policy.Requirements.Add(new MinimumRoleLevelRequirement(RoleLevels.SuperAdmin)));
});

// Add FastEndpoints
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen(); // Enable Swagger with FastEndpoints
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Enable CORS

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();  // Add authorization middleware

app.UseFastEndpoints();

app.Run();
