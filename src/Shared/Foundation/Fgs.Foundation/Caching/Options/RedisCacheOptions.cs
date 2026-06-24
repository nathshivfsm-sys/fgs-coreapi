namespace Fgs.Foundation.Caching.Options;

public sealed class RedisCacheOptions
{
    public const string SectionName = "Redis";

    public bool Enabled { get; set; } = true;

    public string ConnectionString { get; set; } = "localhost:6379";

    public string InstanceName { get; set; } = "fgs:";

    public int DefaultAbsoluteExpirationMinutes { get; set; } = 30;
}
