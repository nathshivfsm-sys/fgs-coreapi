namespace Fgs.File.Application.Abstractions.Storage;

public interface IImageVariantGenerator
{
    Task<GeneratedImageVariant?> GenerateVariantAsync(
        Stream sourceContent,
        string contentType,
        string requestedVariant,
        CancellationToken cancellationToken = default);
}

public sealed record GeneratedImageVariant(
    byte[] Content,
    string ContentType,
    string FileExtension);
