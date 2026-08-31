using Fgs.File.Application.Common.Options;

namespace Fgs.File.Application.Common;

public static class AttachmentFileValidator
{
    public static bool IsAllowedExtension(string fileName, AttachmentValidationOptions options)
    {
        var ext = Path.GetExtension(fileName);
        return !string.IsNullOrWhiteSpace(ext)
               && options.AllowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase);
    }

    public static bool IsAllowedContentType(string contentType, AttachmentValidationOptions options) =>
        options.AllowedContentTypes.Contains(NormalizeMediaType(contentType), StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Clients (Swagger UI, some browsers/Postman) often send <c>application/octet-stream</c>
    /// for binary uploads. Prefer an explicit media type; otherwise map from the file extension.
    /// </summary>
    public static string ResolveContentType(string? contentType, string fileName)
    {
        var normalized = NormalizeMediaType(contentType);
        if (!string.IsNullOrWhiteSpace(normalized)
            && !normalized.Equals("application/octet-stream", StringComparison.OrdinalIgnoreCase))
        {
            return normalized;
        }

        return MapContentTypeFromExtension(fileName) ?? normalized ?? "application/octet-stream";
    }

    public static string NormalizeMediaType(string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
        {
            return string.Empty;
        }

        var mediaType = contentType.Split(';', 2)[0].Trim();
        return mediaType;
    }

    public static string? MapContentTypeFromExtension(string fileName)
    {
        var ext = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(ext))
        {
            return null;
        }

        return ext.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".svg" => "image/svg+xml",
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".zip" => "application/zip",
            ".mp4" => "video/mp4",
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".txt" => "text/plain",
            ".csv" => "text/csv",
            _ => null
        };
    }

    public static bool HasValidMagicBytes(ReadOnlySpan<byte> header, string contentType)
    {
        if (contentType.StartsWith("image/jpeg", StringComparison.OrdinalIgnoreCase))
        {
            return header.Length >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF;
        }

        if (contentType.StartsWith("image/png", StringComparison.OrdinalIgnoreCase))
        {
            return header.Length >= 8
                   && header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47;
        }

        if (contentType.StartsWith("application/pdf", StringComparison.OrdinalIgnoreCase))
        {
            return header.Length >= 4
                   && header[0] == 0x25 && header[1] == 0x50 && header[2] == 0x44 && header[3] == 0x46;
        }

        if (contentType.StartsWith("application/zip", StringComparison.OrdinalIgnoreCase)
            || contentType.StartsWith("application/x-zip-compressed", StringComparison.OrdinalIgnoreCase))
        {
            return header.Length >= 4 && header[0] == 0x50 && header[1] == 0x4B;
        }

        return true;
    }

    public static string SanitizeFileName(string fileName)
    {
        var name = Path.GetFileName(fileName.Trim());
        if (string.IsNullOrWhiteSpace(name) || name is "." or "..")
        {
            return "upload";
        }

        return name;
    }

    public static string BuildStoredFileName(string originalFileName)
    {
        var sanitized = SanitizeFileName(originalFileName);
        var extension = Path.GetExtension(sanitized);
        var baseName = Path.GetFileNameWithoutExtension(sanitized);
        var sanitizedBase = string.Concat(baseName.Where(ch => char.IsLetterOrDigit(ch) || ch is '-' or '_'));
        if (string.IsNullOrWhiteSpace(sanitizedBase))
        {
            sanitizedBase = "upload";
        }

        return $"{sanitizedBase}-{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
    }
}
