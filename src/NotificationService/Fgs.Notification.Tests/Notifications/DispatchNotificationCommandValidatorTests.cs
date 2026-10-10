using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Requests;
using Fgs.Notification.Application.Features.Notifications.Commands.DispatchNotification;

namespace Fgs.Notification.Tests.Notifications;

public sealed class DispatchNotificationCommandValidatorTests
{
    private readonly DispatchNotificationCommandValidator _validator = new();

    [Fact]
    public async Task Validate_EventShapeWithoutTenant_Succeeds()
    {
        var result = await _validator.ValidateAsync(new DispatchNotificationCommand(new DispatchNotificationRequest
        {
            RoutingKey = IntegrationEventRoutingKeys.PasswordReset,
            Payload = "{}",
            MessageId = "msg-1"
        }));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_ExplicitShape_Succeeds()
    {
        var result = await _validator.ValidateAsync(CreateExplicitCommand());

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_NullRequest_Fails()
    {
        var result = await _validator.ValidateAsync(new DispatchNotificationCommand(null!));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_ExplicitShape_MissingTenant_Fails()
    {
        var result = await _validator.ValidateAsync(new DispatchNotificationCommand(new DispatchNotificationRequest
        {
            Channel = "Email",
            TemplateCode = CommunicationTemplateCodes.UserInvitation,
            Recipient = "user@example.com"
        }));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("TenantId"));
    }

    [Fact]
    public async Task Validate_ExplicitShape_InvalidChannel_Fails()
    {
        var result = await _validator.ValidateAsync(new DispatchNotificationCommand(new DispatchNotificationRequest
        {
            TenantId = 1,
            Channel = "Fax",
            TemplateCode = CommunicationTemplateCodes.UserInvitation,
            Recipient = "user@example.com"
        }));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Channel"));
    }

    [Fact]
    public async Task Validate_ExplicitShape_MissingRecipient_Fails()
    {
        var result = await _validator.ValidateAsync(new DispatchNotificationCommand(new DispatchNotificationRequest
        {
            TenantId = 1,
            Channel = "Email",
            TemplateCode = CommunicationTemplateCodes.UserInvitation
        }));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Recipient"));
    }

    private static DispatchNotificationCommand CreateExplicitCommand() =>
        new(new DispatchNotificationRequest
        {
            TenantId = 1,
            CompanyId = 2,
            Channel = "Email",
            TemplateCode = CommunicationTemplateCodes.UserInvitation,
            Recipient = "user@example.com"
        });
}
