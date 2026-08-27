using Fgs.Foundation.Api;

namespace Fgs.Security.Authorization;

public sealed class TenantScopeOptions
{
    public const string SectionName = FgsTenantScopeDefaults.ConfigurationSection;

    public string[] SkipPathPrefixes { get; set; } = [.. FgsTenantScopeDefaults.SkipPathPrefixes];
}
