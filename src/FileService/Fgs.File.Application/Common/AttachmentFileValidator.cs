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
        options.AllowedContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase);

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
