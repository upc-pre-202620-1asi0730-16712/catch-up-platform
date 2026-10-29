using CatchUpPlatform.API.News.Domain.Model.Aggregates;
using CatchUpPlatform.API.News.Domain.Model.Commands;
using CatchUpPlatform.API.News.Application.Errors;
using CatchUpPlatform.API.Shared.Application.Patterns;

namespace CatchUpPlatform.API.News.Application.Services;

/// <summary>
///     Command service interface for favorite source operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling favorite source creation commands.
///     It ensures duplicate detection and persistence of new favorite sources.
/// </remarks>
public interface IFavoriteSourceCommandService
{
    /// <summary>
    ///     Handle the create favorite source command.
    /// </summary>
    /// <remarks>
    ///     This method handles the create favorite source command. It checks if the favorite source already exists for the
    ///     given NewsApiKey and SourceId.
    ///     If it does not exist, it creates a new favorite source and adds it to the database.
    /// </remarks>
    /// <param name="command">CreateFavoriteSourceCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created FavoriteSource on success, or the error reason on failure.
    /// </returns>
    Task<Result<FavoriteSource, CreateFavoriteSourceError>> Handle(CreateFavoriteSourceCommand command, CancellationToken cancellationToken = default);
}
