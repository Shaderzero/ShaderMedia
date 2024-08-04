using System.Text;
using Api.Data;
using Api.Helpers;
using Api.Repositories.Books;
using Api.Repositories.Interfaces.Books;
using Api.Services;
using Api.Services.Books;
using Api.Services.Books.Interfaces;
using Api.Services.Interfaces;
using Api.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Add services to the container.
var connectionString = configuration.GetConnectionString("PostgreSql");
services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(connectionString));
services.AddDbContext<BookDbContext>(options => options.UseNpgsql(connectionString));
services.AddAutoMapper(typeof(AppMappingProfile));

services.AddScoped<IZipService, ZipService>();
services.AddScoped<IAuthorRepository, AuthorRepository>();
services.AddScoped<IBookRepository, BookRepository>();
services.AddScoped<IGenreRepository, GenreRepository>();
services.AddScoped<ILanguageRepository, LanguageRepository>();
services.AddScoped<ISerieRepository, SerieRepository>();
services.AddScoped<IKeywordRepository, KeywordRepository>();
services.AddSingleton<MimeHelper>();

services.AddScoped<IBookService, BookService>();

string? origins = "origins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(origins,
        policy =>
        {
            policy.WithOrigins("https://localhost:7122")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

services.AddHttpContextAccessor();
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(origins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html").AllowAnonymous();

app.Run();