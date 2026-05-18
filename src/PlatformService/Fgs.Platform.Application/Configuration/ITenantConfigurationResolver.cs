using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Application.Configuration;

public interface ITenantConfigurationResolver
{
    TenantProviderConfiguration GetProviderConfiguration(Guid tenantId);

    bool IsFeatureEnabled(Guid tenantId, string featureFlag);
}

public sealed record TenantProviderConfiguration(
    EmailProviderKind EmailProvider,
    string SmsProvider,
    string PushProvider);
