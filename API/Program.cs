using API.Extensions;
using API.Logger;
using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Extensions.Logging;
using Repository;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API - Challenge",
        Description = "A sample ASP.NET Core Web API",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<RepoContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("applicationConnectionString"),
    x =>
    {
        x.MigrationsAssembly("Entities");
        x.EnableRetryOnFailure(3);

    }));
builder.Services.AddCors(options =>
{
   options.AddPolicy("defaultCorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Add Custom Services
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
var config = LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
    loggingBuilder.AddNLog(config.Configuration);
});

builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

var app = builder.Build().MigrateDatabase<RepoContext>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapControllers();



app.Run();


