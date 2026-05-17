using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Application.Notifications.Templates;

public sealed class CommunicationTemplateNotFoundException : Exception
{
    public CommunicationTemplateNotFoundException(
        Guid tenantId,
        Guid? companyId,
        string templateCode,
        NotificationChannel channel)
        : base(
            $"No active communication template '{templateCode}' for channel '{channel}' " +
            $"(tenant '{tenantId}', company '{companyId?.ToString() ?? "—"}', or global fallback).")
    {
        TenantId = tenantId;
        CompanyId = companyId;
        TemplateCode = templateCode;
        Channel = channel;
    }

    public Guid TenantId { get; }

    public Guid? CompanyId { get; }

    public string TemplateCode { get; }

    public NotificationChannel Channel { get; }
}
