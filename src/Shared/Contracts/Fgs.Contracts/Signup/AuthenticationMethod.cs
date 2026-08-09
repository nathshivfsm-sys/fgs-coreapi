namespace Fgs.Contracts.Signup;

/// <summary>
/// How the user authenticates. Matches identity.FgsUser.AuthenticationMethod.
/// </summary>
public enum AuthenticationMethod : short
{
    Password = 1,

    EmailOtp = 2,

    PasswordOrEmailOtp = 3,

    EntraIdOnly = 4,

    PasswordWithMfa = 5
}
