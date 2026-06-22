namespace Fgs.Security.Abstractions;

public interface IFgsUserContext
{
    bool IsAuthenticated { get; }

    Guid? UserId { get; }

    string? Email { get; }

    /// <summary>
    /// Human-readable user name for audit fields (CreatedBy/UpdatedBy).
    /// </summary>
    string? DisplayName { get; }

    string? EntraObjectId { get; }

    long? TenantId { get; }

    long? CompanyId { get; }

    IReadOnlyList<string> Roles { get; }

    bool IsInRole(string roleCode);
}
