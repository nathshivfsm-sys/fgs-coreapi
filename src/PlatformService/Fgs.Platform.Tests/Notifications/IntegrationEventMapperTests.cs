using Fgs.Platform.Infrastructure.Options;
using Fgs.Platform.Domain.Notifications;
using System.Text.Json;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Platform.Infrastructure.Notifications.Queues;
using Fgs.Messaging.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Platform.Tests.Notifications;

public sealed class IntegrationEventMapperTests
{
    private readonly IntegrationEventMapper _mapper = new(
        Options.Create(new NotificationOptions
        {
            PlatformName = "FGS",
            SupportEmail = "support@fgs.example",
            CompanyName = "FGS",
            InvitationExpirationHours = 72
        }));

    [Fact]
    public void CanMap_KnownUserServiceRoutingKeys()
    {
        _mapper.CanMap(IntegrationEventRoutingKeys.CompanySignupInviteEmail).Should().BeTrue();
        _mapper.CanMap(IntegrationEventRoutingKeys.UserInvited).Should().BeTrue();
        _mapper.CanMap(IntegrationEventRoutingKeys.PasswordReset).Should().BeTrue();
        _mapper.CanMap(IntegrationEventRoutingKeys.CompanyCreated).Should().BeTrue();
    }

    [Fact]
    public void Map_CompanySignupInviteEmail_BuildsEmailDispatchRequest()
    {
        const long tenantId = 5001;
        const long companyId = 42;
        var payload = JsonSerializer.Serialize(new CompanySignupInviteEmailEvent(
            tenantId,
            companyId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            "invite@example.com",
            CommunicationTemplateCodes.CompanyAdminInvitation,
            "Alex",
            "Acme Platform",
            "https://example.com/invite",
            "48",
            "help@acme.example"));

        var request = _mapper.Map(
            IntegrationEventRoutingKeys.CompanySignupInviteEmail,
            payload,
            "corr",
            "mid");

        request.Should().NotBeNull();
        request!.TenantId.Should().Be(tenantId);
        request.CompanyId.Should().Be(companyId);
        request.Channel.Should().Be(NotificationChannel.Email);
        request.Recipient.Should().Be("invite@example.com");
        request.TemplateCode.Should().Be(CommunicationTemplateCodes.CompanyAdminInvitation);
        request.TemplateData["Name"].Should().Be("Alex");
        request.TemplateData["InviteLink"].Should().Be("https://example.com/invite");
        request.TemplateData["PlatformName"].Should().Be("Acme Platform");
        request.TemplateData["ExpirationHours"].Should().Be("48");
        request.TemplateData["CompanyName"].Should().Be("FGS");
        request.TemplateData["SupportEmail"].Should().Be("help@acme.example");
        request.TemplateData["FgsTenantId"].Should().Be("5001");
    }

    [Fact]
    public void Map_CompanySignupInviteEmail_UsesNotificationConfigWhenEventTokensEmpty()
    {
        var payload = JsonSerializer.Serialize(new CompanySignupInviteEmailEvent(
            42,
            1,
            Guid.NewGuid(),
            Guid.NewGuid(),
            "invite@example.com",
            CommunicationTemplateCodes.CompanyAdminInvitation,
            "Alex",
            string.Empty,
            "https://example.com/invite",
            string.Empty,
            string.Empty));

        var request = _mapper.Map(
            IntegrationEventRoutingKeys.CompanySignupInviteEmail,
            payload,
            null,
            "mid");

        request!.TemplateData["PlatformName"].Should().Be("FGS");
        request.TemplateData["SupportEmail"].Should().Be("support@fgs.example");
        request.TemplateData["CompanyName"].Should().Be("FGS");
        request.TemplateData["ExpirationHours"].Should().Be("72");
    }

    [Fact]
    public void Map_CompanySignupInviteEmail_AcceptsLegacyGuidCompanyId()
    {
        const string payload =
            """
            {
              "TenantId": 5001,
              "CompanyId": "b57d16aa-9eba-4867-9e5e-48ac84597e14",
              "UserId": "11111111-1111-1111-1111-111111111111",
              "InvitationId": "22222222-2222-2222-2222-222222222222",
              "Email": "invite@example.com",
              "EmailTemplateCode": "COMPANY_ADMIN_INVITATION",
              "Name": "Alex",
              "PlatformName": "",
              "InviteLink": "https://example.com/invite",
              "ExpirationHours": "72",
              "SupportEmail": ""
            }
            """;

        var request = _mapper.Map(
            IntegrationEventRoutingKeys.CompanySignupInviteEmail,
            payload,
            "corr",
            "mid");

        request.Should().NotBeNull();
        request!.TenantId.Should().Be(5001);
        request.CompanyId.Should().BeNull();
        request.Recipient.Should().Be("invite@example.com");
    }
}
