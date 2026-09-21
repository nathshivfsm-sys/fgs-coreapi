namespace Fgs.Contracts.Clients;

/// <summary>
/// Batch enrichment for Setup employee list/detail (primary role + last login).
/// </summary>
public sealed record FgsUserListEnrichmentDto(
    Guid UserId,
    long? RoleId,
    string? RoleName,
    DateTimeOffset? LastLoginOn);
