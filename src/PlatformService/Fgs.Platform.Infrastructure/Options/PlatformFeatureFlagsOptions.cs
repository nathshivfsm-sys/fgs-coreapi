namespace Fgs.Platform.Infrastructure.Options;

public sealed class PlatformFeatureFlagsOptions
{
    public const string SectionName = "FeatureFlags";

    public Dictionary<string, bool> Global { get; set; } = new();

    public Dictionary<string, Dictionary<string, bool>> Tenants { get; set; } = new();
}
