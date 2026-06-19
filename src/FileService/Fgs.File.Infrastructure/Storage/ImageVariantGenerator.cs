using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Fgs.File.Infrastructure.Storage;

public sealed class ImageVariantGenerator : IImageVariantGenerator
{
    public async Task<IReadOnlyDictionary<string, GeneratedImageVariant>> GenerateVariantsAsync(
        Stream sourceContent,
        string contentType,
        IReadOnlyList<string> requestedVariants,
        CancellationToken cancellationToken = default)
    {
        await using var memoryStream = new MemoryStream();
        await sourceContent.CopyToAsync(memoryStream, cancellationToken);
        var bytes = memoryStream.ToArray();

        if (contentType.Contains("svg", StringComparison.OrdinalIgnoreCase))
        {
            return requestedVariants.ToDictionary(
                variant => variant.ToLowerInvariant(),
                variant => new GeneratedImageVariant(bytes, contentType, ".svg"),
                StringComparer.OrdinalIgnoreCase);
        }

        using var image = await Image.LoadAsync(new MemoryStream(bytes), cancellationToken);
        var results = new Dictionary<string, GeneratedImageVariant>(StringComparer.OrdinalIgnoreCase);

        foreach (var variant in requestedVariants)
        {
            if (!FileLogoVariants.MaxDimensions.TryGetValue(variant, out var maxSize))
            {
                continue;
            }

            using var clone = image.Clone(ctx => ctx.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(maxSize, maxSize)
            }));

            await using var output = new MemoryStream();
            var encoder = ResolveEncoder(contentType);
            await clone.SaveAsync(output, encoder, cancellationToken);

            results[variant.ToLowerInvariant()] = new GeneratedImageVariant(
                output.ToArray(),
                contentType,
                ResolveExtension(contentType));
        }

        return results;
    }

    private static IImageEncoder ResolveEncoder(string contentType)
    {
        if (contentType.Contains("webp", StringComparison.OrdinalIgnoreCase))
        {
            return new WebpEncoder();
        }

        if (contentType.Contains("jpeg", StringComparison.OrdinalIgnoreCase)
            || contentType.Contains("jpg", StringComparison.OrdinalIgnoreCase))
        {
            return new JpegEncoder();
        }

        return new PngEncoder();
    }

    private static string ResolveExtension(string contentType)
    {
        if (contentType.Contains("webp", StringComparison.OrdinalIgnoreCase))
        {
            return ".webp";
        }

        if (contentType.Contains("jpeg", StringComparison.OrdinalIgnoreCase)
            || contentType.Contains("jpg", StringComparison.OrdinalIgnoreCase))
        {
            return ".jpg";
        }

        return ".png";
    }
}
