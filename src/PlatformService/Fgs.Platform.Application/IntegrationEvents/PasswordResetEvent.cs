namespace Fgs.Platform.Application.IntegrationEvents;

public sealed record PasswordResetEvent(
    Guid TenantId,
    string Email,
    string DisplayName,
    string ResetUrl);
