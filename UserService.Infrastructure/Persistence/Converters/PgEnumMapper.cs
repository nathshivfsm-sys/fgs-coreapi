using UserService.Domain.Enums;

namespace UserService.Infrastructure.Persistence.Converters;

internal static class PgEnumMapper
{
    public static string TenantStatusToPg(TenantStatus value) =>
        value switch
        {
            TenantStatus.Active => "active",
            TenantStatus.Suspended => "suspended",
            TenantStatus.Closed => "closed",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    public static TenantStatus TenantStatusFromPg(string value) =>
        value switch
        {
            "active" => TenantStatus.Active,
            "suspended" => TenantStatus.Suspended,
            "closed" => TenantStatus.Closed,
            _ => throw new InvalidOperationException($"Unknown tenant status '{value}'.")
        };

    public static string UserStatusToPg(UserStatus value) =>
        value switch
        {
            UserStatus.Pending => "pending",
            UserStatus.Active => "active",
            UserStatus.Suspended => "suspended",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    public static UserStatus UserStatusFromPg(string value) =>
        value switch
        {
            "pending" => UserStatus.Pending,
            "active" => UserStatus.Active,
            "suspended" => UserStatus.Suspended,
            _ => throw new InvalidOperationException($"Unknown user status '{value}'.")
        };

    public static string InviteStatusToPg(InviteStatus value) =>
        value switch
        {
            InviteStatus.Pending => "pending",
            InviteStatus.Accepted => "accepted",
            InviteStatus.Revoked => "revoked",
            InviteStatus.Expired => "expired",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    public static InviteStatus InviteStatusFromPg(string value) =>
        value switch
        {
            "pending" => InviteStatus.Pending,
            "accepted" => InviteStatus.Accepted,
            "revoked" => InviteStatus.Revoked,
            "expired" => InviteStatus.Expired,
            _ => throw new InvalidOperationException($"Unknown invite status '{value}'.")
        };

    public static string UserRoleToPg(UserRole value) =>
        value switch
        {
            UserRole.Member => "member",
            UserRole.Admin => "admin",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    public static UserRole UserRoleFromPg(string value) =>
        value switch
        {
            "member" => UserRole.Member,
            "admin" => UserRole.Admin,
            _ => throw new InvalidOperationException($"Unknown user role '{value}'.")
        };
}
