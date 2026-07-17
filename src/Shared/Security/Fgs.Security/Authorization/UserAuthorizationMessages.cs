namespace Fgs.Security.Authorization;

public static class UserAuthorizationMessages
{
    public const string ProfileNotFound = "Your account could not be verified. Please sign in again.";
    public const string UserInactive = "Your account is not active. Contact your administrator.";
    public const string UserDeleted = "Your account is no longer available. Contact your administrator.";
    public const string TenantScopeMissing = "Tenant context is required. Include the X-Tenant-Id header.";
    public const string CompanyScopeMissing = "Company context is required. Include the X-Company-Id header.";
    public const string TenantMismatch = "You do not have access to this tenant.";
    public const string CompanyMismatch = "You do not have access to this company.";
    public const string RouteTenantMismatch = "The requested tenant does not match your account.";
    public const string RouteCompanyMismatch = "The requested company does not match your account.";
    public const string InsufficientRole = "You do not have permission to perform this action.";
}
