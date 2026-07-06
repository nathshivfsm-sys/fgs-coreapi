namespace Fgs.File.Application.Common.Options;

public sealed class AttachmentValidationOptions
{
    public const string SectionName = "AttachmentValidation";

    public string[] AllowedExtensions { get; set; } =
    [
        ".jpg", ".jpeg", ".png", ".webp", ".gif", ".bmp",
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
        ".zip", ".mp4", ".mp3", ".wav", ".txt", ".csv"
    ];

    public string[] AllowedContentTypes { get; set; } =
    [
        "image/jpeg", "image/png", "image/webp", "image/gif", "image/bmp", "image/svg+xml",
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-excel",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "application/vnd.ms-powerpoint",
        "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        "application/zip", "application/x-zip-compressed",
        "video/mp4", "audio/mpeg", "audio/wav", "audio/x-wav",
        "text/plain", "text/csv"
    ];
}
