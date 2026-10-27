using CatchUpPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Database Context and route EF logs through the app logger pipeline.
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");
    
    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");
    
    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();

});





var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();