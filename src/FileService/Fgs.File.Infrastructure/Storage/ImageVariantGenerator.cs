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
    public async Task<GeneratedImageVariant?> GenerateVariantAsync(
        Stream sourceContent,
        string contentType,
        string requestedVariant,
        CancellationToken cancellationToken = default)
    {
        var variant = requestedVariant.ToLowerInvariant();
        if (!FileLogoVariants.IsSupported(variant))
        {
            return null;
        }

        await using var memoryStream = new MemoryStream();
        await sourceContent.CopyToAsync(memoryStream, cancellationToken);
        var bytes = memoryStream.ToArray();

        if (contentType.Contains("svg", StringComparison.OrdinalIgnoreCase))
        {
            return new GeneratedImageVariant(bytes, contentType, ".svg");
        }

        if (!FileLogoVariants.MaxDimensions.TryGetValue(variant, out var maxSize))
        {
            return null;
        }

        using var image = await Image.LoadAsync(new MemoryStream(bytes), cancellationToken);
        using var clone = image.Clone(ctx => ctx.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(maxSize, maxSize)
        }));

        await using var output = new MemoryStream();
        var encoder = ResolveEncoder(contentType);
        await clone.SaveAsync(output, encoder, cancellationToken);

        return new GeneratedImageVariant(
            output.ToArray(),
            contentType,
            ResolveExtension(contentType));
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
