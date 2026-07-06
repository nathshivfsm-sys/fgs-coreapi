namespace Fgs.File.Application.Abstractions.Storage;

public interface IThumbnailGenerator
{
    Task<GeneratedThumbnail?> GenerateAsync(
        Stream sourceContent,
        string contentType,
        string originalFileName,
        CancellationToken cancellationToken = default);
}

public sealed record GeneratedThumbnail(
    byte[] Content,
    string ContentType,
    string ThumbnailFileName);
