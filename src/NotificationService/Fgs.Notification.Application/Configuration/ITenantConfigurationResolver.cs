using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Application.Configuration;

public interface ITenantConfigurationResolver
{
    TenantProviderConfiguration GetProviderConfiguration(long tenantId);

    bool IsFeatureEnabled(long tenantId, string featureFlag);
}

public sealed record TenantProviderConfiguration(
    EmailProviderKind EmailProvider,
    string SmsProvider,
    string PushProvider);
