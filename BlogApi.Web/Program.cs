using BlogApi.Application.UseCases;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Data;
using BlogApi.Infrastructure.Repositories;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<GetArticlesUseCase>();

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

app.UseFastEndpoints();

app.Run();
