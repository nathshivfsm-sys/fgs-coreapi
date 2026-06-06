using Fgs.Notification.Infrastructure.Options;
namespace Fgs.Notification.Infrastructure.Options;

public sealed class NotificationFeatureFlagsOptions
{
    public const string SectionName = "FeatureFlags";

    public Dictionary<string, bool> Global { get; set; } = new();

    public Dictionary<string, Dictionary<string, bool>> Tenants { get; set; } = new();
}
