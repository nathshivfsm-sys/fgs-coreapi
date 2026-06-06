using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Application.Notifications.Templates;

public sealed class CommunicationTemplateNotFoundException : Exception
{
    public CommunicationTemplateNotFoundException(
        long tenantId,
        long? companyId,
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

    public long TenantId { get; }

    public long? CompanyId { get; }

    public string TemplateCode { get; }

    public NotificationChannel Channel { get; }
}
