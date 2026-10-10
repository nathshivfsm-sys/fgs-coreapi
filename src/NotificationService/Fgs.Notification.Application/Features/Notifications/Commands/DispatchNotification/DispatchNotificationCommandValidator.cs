using FluentValidation;
using Fgs.Contracts.Requests;
using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Application.Features.Notifications.Commands.DispatchNotification;

public sealed class DispatchNotificationCommandValidator : AbstractValidator<DispatchNotificationCommand>
{
    public DispatchNotificationCommandValidator()
    {
        RuleFor(x => x.Request).NotNull();

        When(x => x.Request is not null && !IsEventDispatch(x.Request), () =>
        {
            RuleFor(x => x.Request!.TenantId)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.Request!.Channel)
                .Must(channel => Enum.TryParse<NotificationChannel>(channel, ignoreCase: true, out _))
                .WithMessage("Channel must be a valid notification channel.");

            RuleFor(x => x.Request!.TemplateCode).NotEmpty();
            RuleFor(x => x.Request!.Recipient).NotEmpty();
        });
    }

    private static bool IsEventDispatch(DispatchNotificationRequest request) =>
        !string.IsNullOrWhiteSpace(request.RoutingKey)
        && !string.IsNullOrWhiteSpace(request.Payload)
        && !string.IsNullOrWhiteSpace(request.MessageId);
}
