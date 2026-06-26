namespace Fgs.Foundation.Compression.Options;

public sealed class FgsResponseCompressionOptions
{
    public const string SectionName = "ResponseCompression";

    public bool Enabled { get; set; } = true;

    public bool EnableForHttps { get; set; } = true;
}
