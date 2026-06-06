using Fgs.Contracts.Api;
using Fgs.Contracts.Requests;
using MediatR;

namespace Fgs.Notification.Application.Features.Notifications.Commands.DispatchNotification;

public sealed record DispatchNotificationCommand(DispatchNotificationRequest Request)
    : IRequest<ApiResponse<object>>;
