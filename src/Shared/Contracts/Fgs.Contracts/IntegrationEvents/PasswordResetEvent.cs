namespace Fgs.Contracts.IntegrationEvents;

public sealed record PasswordResetEvent(
    long TenantId,
    long CompanyId,
    Guid UserId,
    string Email,
    string DisplayName,
    string ResetUrl);
