namespace CatchUpPlatform.API.News.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a favorite source.
/// </summary>
public enum CreateFavoriteSourceError
{
	/// <summary>
	/// The provided news API key is invalid.
	/// </summary>
	InvalidNewsApiKey,

	/// <summary>
	/// The provided source ID is invalid.
	/// </summary>
	InvalidSourceId,

	/// <summary>
	/// A favorite source with the same API key and source ID already exists.
	/// </summary>
	DuplicateFavoriteSource,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}

