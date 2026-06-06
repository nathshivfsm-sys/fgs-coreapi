using FluentValidation;
using Fgs.Contracts.Requests;

namespace Fgs.Notification.Application.Features.Notifications.Commands.DispatchNotification;

public sealed class DispatchNotificationCommandValidator : AbstractValidator<DispatchNotificationCommand>
{
    public DispatchNotificationCommandValidator()
    {
        RuleFor(x => x.Request).NotNull();
    }
}
