namespace Fgs.User.Application.Features.Signup;

public static class SignupErrorMessages
{
    public const string BusinessTypeIdsRequired = "At least one industry (business type) must be selected.";

    public const string UniqueTenantCodeFailed =
        "Unable to generate a unique tenant code. Please try a different company name.";

    public const string EmailAlreadyUsed =
        "This email address is already associated with an account or pending invitation.";

    public const string InvalidPhoneFormat = "Phone number format is invalid.";
}
