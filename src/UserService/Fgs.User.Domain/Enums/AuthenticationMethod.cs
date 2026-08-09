namespace Fgs.User.Domain.Enums;

/// <summary>
/// How the user authenticates. Stored as smallint on identity.FgsUser.
/// </summary>
public enum AuthenticationMethod : short
{
    Password = 1,

    EmailOtp = 2,

    PasswordOrEmailOtp = 3,

    EntraIdOnly = 4,

    PasswordWithMfa = 5
}
