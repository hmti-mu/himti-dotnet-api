using BlogApi.Application.UseCases;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Data;
using BlogApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<GetArticlesUseCase>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger configuration

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
    app.UseSwagger(); // Enable Swagger
    app.UseSwaggerUI(); // Enable SwaggerUI
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Enable CORS
app.MapControllers(); // Map controller routes

app.MapGet(
        "/api/articles",
        async (GetArticlesUseCase useCase) =>
        {
            var articles = await useCase.ExecuteAsync();
            return Results.Ok(articles);
        }
    )
    .WithName("GetArticles");

app.Run();
