namespace Fgs.Contracts.IntegrationEvents;

public static class IntegrationEventTypes
{
    public const string CompanySignupInviteEmail = "CompanySignupInviteEmail";

    public const string TenantProvisionRequested = "TenantProvisionRequested";

    public const string UserInvited = "UserInvited";

    public const string PasswordReset = "PasswordReset";

    public const string CompanyCreated = "CompanyCreated";

    public static class AggregateTypes
    {
        public const string Invitation = "Invitation";

        public const string Tenant = "Tenant";

        public const string User = "User";

        public const string Company = "Company";
    }
}
