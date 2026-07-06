using Fgs.File.Application.Abstractions.Storage;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Fgs.File.Infrastructure.Storage;

public sealed class AttachmentThumbnailGenerator : IThumbnailGenerator
{
    private const int MaxThumbnailDimension = 256;

    public async Task<GeneratedThumbnail?> GenerateAsync(
        Stream sourceContent,
        string contentType,
        string originalFileName,
        CancellationToken cancellationToken = default)
    {
        await using var memoryStream = new MemoryStream();
        await sourceContent.CopyToAsync(memoryStream, cancellationToken);
        var bytes = memoryStream.ToArray();

        if (IsImageContentType(contentType))
        {
            return await GenerateImageThumbnailAsync(bytes, contentType, originalFileName, cancellationToken);
        }

        return GenerateIconThumbnail(originalFileName);
    }

    private static bool IsImageContentType(string contentType) =>
        contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
        && !contentType.Contains("svg", StringComparison.OrdinalIgnoreCase);

    private static async Task<GeneratedThumbnail> GenerateImageThumbnailAsync(
        byte[] bytes,
        string contentType,
        string originalFileName,
        CancellationToken cancellationToken)
    {
        using var image = await Image.LoadAsync(new MemoryStream(bytes), cancellationToken);
        using var clone = image.Clone(ctx => ctx.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(MaxThumbnailDimension, MaxThumbnailDimension)
        }));

        await using var output = new MemoryStream();
        var encoder = ResolveEncoder(contentType);
        await clone.SaveAsync(output, encoder, cancellationToken);

        var ext = Path.GetExtension(originalFileName);
        if (string.IsNullOrWhiteSpace(ext))
        {
            ext = ResolveExtension(contentType);
        }

        var baseName = Path.GetFileNameWithoutExtension(originalFileName);
        return new GeneratedThumbnail(
            output.ToArray(),
            contentType,
            $"{baseName}_thumbnail{ext}");
    }

    private static GeneratedThumbnail GenerateIconThumbnail(string originalFileName)
    {
        using var image = new Image<Rgba32>(MaxThumbnailDimension, MaxThumbnailDimension, new Rgba32(220, 220, 220, 255));
        using var output = new MemoryStream();
        image.Save(output, new PngEncoder());

        var baseName = Path.GetFileNameWithoutExtension(originalFileName);
        return new GeneratedThumbnail(
            output.ToArray(),
            "image/png",
            $"{baseName}_thumbnail.png");
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
