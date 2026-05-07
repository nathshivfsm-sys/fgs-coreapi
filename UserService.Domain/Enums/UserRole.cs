namespace UserService.Domain.Enums;

/// <summary>
/// Application role for directory users. Stored in <c>fgs.users.role</c> (added for admin signup; extend as needed).
/// </summary>
public enum UserRole
{
    Member,
    Admin
}
