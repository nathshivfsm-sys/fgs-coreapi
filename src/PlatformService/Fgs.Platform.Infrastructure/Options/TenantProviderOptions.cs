using Fgs.Platform.Infrastructure.Options;
using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Infrastructure.Options;

public sealed class TenantProviderOptions
{
    public const string SectionName = "TenantProviders";

    public TenantProviderBindingOptions Default { get; set; } = new();

    public Dictionary<string, TenantProviderBindingOptions> Tenants { get; set; } = new();
}

public sealed class TenantProviderBindingOptions
{
    public string Email { get; set; } = nameof(EmailProviderKind.SendGrid);

    public string Sms { get; set; } = "Twilio";

    public string Push { get; set; } = "Firebase";
}
