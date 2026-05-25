namespace Fgs.User.Application.IntegrationEvents;

public static class IntegrationEventTypes
{
    public const string CompanySignupInviteEmail = "CompanySignupInviteEmail";

    public const string TenantProvisionRequested = "TenantProvisionRequested";

    public static class AggregateTypes
    {
        public const string Invitation = "Invitation";

        public const string Tenant = "Tenant";
    }
}
