using InfoTrackProject;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Setup CORS to allow all origins
builder.Services.AddCors(x =>
{
    x.AddPolicy("AllowAny", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// DI
builder.Services.AddSingleton<IScraper, Scraper>();
builder.Services.AddSingleton<IGoogleSearchPageInfoExtractor, GoogleSearchPageInfoExtractor>();
builder.Services.AddSingleton<IGoogleResultsScraperService, GoogleResultsScraperService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAny");

app.Run();