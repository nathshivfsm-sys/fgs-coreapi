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

    public const string Unauthenticated = "Authentication is required.";

    public const string UserNotFound = "User profile was not found.";

    public const string LoginNotAvailable =
        "Sign-in is not available for this account. Use the invitation link from your signup email or contact your administrator.";

    public const string UserNotActive = "User account is not active.";

    public const string InvalidLoginOAuthState = "Invalid login OAuth state.";

    public const string InvitationNotAccepted =
        "Sign-in requires an accepted invitation. Use the invitation link from your email.";

    public const string TenantNotActive = "Tenant is not active.";

    public const string CompanyNotActive = "Company is not active.";

    public const string RefreshTokenRequired = "Refresh token is required.";

    public const string RefreshTokenFailed = "Failed to refresh the access token.";

    public const string PkceStateExpired = "Login session expired. Restart sign-in.";
}
