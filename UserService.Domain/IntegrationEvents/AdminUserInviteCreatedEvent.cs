namespace UserService.Domain.IntegrationEvents;

/// <summary>
/// Published after the company signup transaction commits. Downstream consumers send the invite email.
/// </summary>
public sealed record AdminUserInviteCreatedEvent(
    string TenantName,
    string Email,
    string InviteToken);
