using CatchUpPlatform.API.News.Application.Errors;
using CatchUpPlatform.API.News.Application.Services;
using CatchUpPlatform.API.News.Domain.Model.Aggregates;
using CatchUpPlatform.API.News.Domain.Model.Commands;
using CatchUpPlatform.API.News.Domain.Repositories;
using CatchUpPlatform.API.Shared.Application.Patterns;
using CatchUpPlatform.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CatchUpPlatform.API.News.Application.Internal.CommandServices;

public class FavoriteSourceCommandService(
    IFavoriteSourceRepository favoriteSourceRepository,
    IUnitOfWork unitOfWork,
    ILogger<FavoriteSourceCommandService> logger)
: IFavoriteSourceCommandService
{
    public async Task<Result<FavoriteSource, CreateFavoriteSourceError>> Handle(CreateFavoriteSourceCommand command, CancellationToken cancellationToken = default)
    {

        var existingFavoriteSource = await favoriteSourceRepository.FindByNewsApiKeyAndSourceIdAsync(
            command.NewsApiKey, command.SourceId, cancellationToken);
        if(existingFavoriteSource != null)
        {
            logger.LogWarning("Duplicate favorite source creation attempt for NewsApiKey: {NewsApiKey}, SourceId: {SourceId}", 
                command.NewsApiKey.MaskedForLogging(), 
                command.SourceId);
            return new Result<FavoriteSource, CreateFavoriteSourceError>.Failure(CreateFavoriteSourceError.DuplicateFavoriteSource);
        }
        
        try
        {
            var favoriteSource = new FavoriteSource(command);
            await favoriteSourceRepository.AddAsync(favoriteSource, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<FavoriteSource, CreateFavoriteSourceError>.Success(favoriteSource);
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, "Invalid argument provided when creating favorite source for NewsApiKey: {NewsApiKey}, SourceId: {SourceId}", 
                command.NewsApiKey.MaskedForLogging(), 
                command.SourceId);

            return new Result<FavoriteSource, CreateFavoriteSourceError>.Failure(CreateFavoriteSourceError
                .UnexpectedError);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "A database update error occurred when creating favorite source for NewsApiKey: {NewsApiKey}, SourceId: {SourceId}", 
                command.NewsApiKey.MaskedForLogging(), 
                command.SourceId);
            return new Result<FavoriteSource, CreateFavoriteSourceError>.Failure(CreateFavoriteSourceError
                .UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error creating favorite source for NewsApiKey: {NewsApiKey}, SourceId: {SourceId}", 
                command.NewsApiKey.MaskedForLogging(), 
                command.SourceId);

            return new Result<FavoriteSource, CreateFavoriteSourceError>.Failure(CreateFavoriteSourceError
                .UnexpectedError);
        }
        
    }
}