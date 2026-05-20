namespace Fgs.User.Application.Features.Signup;

/// <summary>
/// Well-known values used when persisting self-serve company signup records.
/// </summary>
public static class SignupConstants
{
    /// <summary>
    /// Audit actor for records created through the prospect signup flow.
    /// </summary>
    public const string ProspectActor = "Prospect";

    public const int DefaultLanguageId = 1;
}
