namespace Fgs.File.Application.Common.Options;

public sealed class FileServiceOptions
{
    public const string SectionName = "FileService";

    public string PublicBaseUrl { get; set; } = "https://developer.fsm.com";

    public int UploadUrlExpiryMinutes { get; set; } = 15;

    public int DownloadUrlExpiryMinutes { get; set; } = 15;

    public long MaxUploadSizeBytes { get; set; } = 10 * 1024 * 1024;

    public int StreamCacheControlMaxAgeSeconds { get; set; } = 3600;
}
