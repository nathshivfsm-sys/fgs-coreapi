namespace Fgs.File.Application.Abstractions.Storage;

public interface IImageVariantGenerator
{
    Task<IReadOnlyDictionary<string, GeneratedImageVariant>> GenerateVariantsAsync(
        Stream sourceContent,
        string contentType,
        IReadOnlyList<string> requestedVariants,
        CancellationToken cancellationToken = default);
}

public sealed record GeneratedImageVariant(
    byte[] Content,
    string ContentType,
    string FileExtension);
