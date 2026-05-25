namespace Fgs.User.Application.Features.Auth;

public static class AuthErrorMessages
{
    public const string InvalidOAuthState = "Invalid OAuth state.";

    public const string EntraCodeExchangeFailed = "Failed to exchange authorization code with Entra.";

    public const string InvitationNotFound = "Invitation not found.";

    public const string InvitationNotActive = "Invitation is not active.";

    public const string EntraEmailMismatch = "Entra account email does not match the invitation.";

    public const string FinalizeOnboardingFailed = "Failed to finalize onboarding.";

    public const string InvitationUserNotFound = "Invitation user not found.";

    public const string TenantNotFound = "Tenant was not found.";

    public const string TenantCompanyNotFound = "Tenant company was not found.";
}
