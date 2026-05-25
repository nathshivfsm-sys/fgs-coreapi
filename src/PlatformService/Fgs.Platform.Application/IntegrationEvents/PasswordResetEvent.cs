namespace Fgs.Platform.Application.IntegrationEvents;

public sealed record PasswordResetEvent(
    long TenantId,
    string Email,
    string DisplayName,
    string ResetUrl);
