using Fgs.Notification.Application.Configuration;
using Fgs.Notification.Domain.Notifications;
using Fgs.Notification.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Notification.Infrastructure.Configuration;

public sealed class TenantConfigurationResolver(IOptions<TenantProviderOptions> tenantProviders)
    : ITenantConfigurationResolver
{
    public TenantProviderConfiguration GetProviderConfiguration(long tenantId)
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

    private static EmailProviderKind ParseEmailProvider(string value) =>
        Enum.TryParse<EmailProviderKind>(value, ignoreCase: true, out var kind)
            ? kind
            : EmailProviderKind.SendGrid;
}
