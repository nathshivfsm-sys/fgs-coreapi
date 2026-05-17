using Fgs.Platform.Domain.Entities;
using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Infrastructure.Database.Seed;

public static class CommunicationTemplateSeedData
{
    public const long CompanyAdminInvitationTemplateId = 1;

    private static readonly DateTimeOffset SeedTimestamp =
        new(2026, 5, 17, 0, 0, 0, TimeSpan.Zero);

    public const string CompanyAdminInvitationSubject =
        "Welcome to {{PlatformName}} – Activate Your Admin Account";

    public const string CompanyAdminInvitationBody =
        """
        Hello {{Name}},

        Welcome to {{PlatformName}}.

        Your company account has been created, and you have been assigned as the administrator for your organization.

        To complete your setup and activate your administrator account, please click the link below:
        {{InviteLink}}

        During setup, you will be asked to:
        • Create or sign in to your account
        • Verify your email address

        The invite link will expire in {{ExpirationHours}} hours.

        If you did not expect this invitation or believe you received it in error, please ignore this email or contact support.

        Thank you,
        {{CompanyName}}
        {{SupportEmail}}
        """;

    public static FgsSetupCommunicationTemplate CompanyAdminInvitationEmail() => new()
    {
        Id = CompanyAdminInvitationTemplateId,
        TenantId = null,
        CompanyId = null,
        TemplateType = CommunicationTemplateTypes.Email,
        Code = CommunicationTemplateCodes.CompanyAdminInvitation,
        Name = "Company Admin Invitation Email",
        Subject = CompanyAdminInvitationSubject,
        Body = CompanyAdminInvitationBody,
        IsMobileVisible = false,
        IsActive = true,
        CreatedOn = SeedTimestamp
    };
}
