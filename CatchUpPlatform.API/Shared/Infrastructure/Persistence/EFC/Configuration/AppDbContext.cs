using CatchUpPlatform.API.News.Domain.Model.Aggregates;
using CatchUpPlatform.API.News.Domain.Model.ValueObjects;
using CatchUpPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using CatchUpPlatform.API.Shared.Infrastructure.Persistence.EFC.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace CatchUpPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context
/// </summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<FavoriteSource>().HasKey(f => f.Id);
        builder.Entity<FavoriteSource>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<FavoriteSource>().Property(f => f.SourceId)
            .HasConversion(v => v.Value, v => new SourceId(v))
            .IsRequired();

        builder.Entity<FavoriteSource>()
            .Property(f => f.NewsApiKey)
            .HasConversion(v => v.Value, v => new NewsApiKey(v))
            .IsRequired();

        builder.Entity<FavoriteSource>()
            .HasIndex(f => new { f.NewsApiKey, f.SourceId })
            .IsUnique();
        
        builder.UseSnakeCaseNamingConvention();
    }
}