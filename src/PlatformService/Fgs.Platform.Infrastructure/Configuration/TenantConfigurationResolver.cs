using Fgs.Platform.Application.Configuration;
using Fgs.Platform.Domain.Notifications;
using Fgs.Platform.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Platform.Infrastructure.Configuration;

public sealed class TenantConfigurationResolver(
    IOptions<TenantProviderOptions> tenantProviders,
    IOptions<PlatformFeatureFlagsOptions> featureFlags) : ITenantConfigurationResolver
{
    public TenantProviderConfiguration GetProviderConfiguration(Guid tenantId)
    {
        var options = tenantProviders.Value;
        var binding = options.Tenants.TryGetValue(tenantId.ToString(), out var tenantBinding)
            ? tenantBinding
            : options.Default;

        return new TenantProviderConfiguration(
            ParseEmailProvider(binding.Email),
            binding.Sms,
            binding.Push);
    }

    public bool IsFeatureEnabled(Guid tenantId, string featureFlag)
    {
        var flags = featureFlags.Value;
        if (flags.Tenants.TryGetValue(tenantId.ToString(), out var tenantFlags)
            && tenantFlags.TryGetValue(featureFlag, out var tenantValue))
        {
            return tenantValue;
        }

        return flags.Global.TryGetValue(featureFlag, out var globalValue) && globalValue;
    }

    private static EmailProviderKind ParseEmailProvider(string value) =>
        Enum.TryParse<EmailProviderKind>(value, ignoreCase: true, out var kind)
            ? kind
            : EmailProviderKind.SendGrid;
}
