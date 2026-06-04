using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Notification.Application.Features.Notifications.Commands.ProcessNotificationQueueMessage;

public sealed record ProcessNotificationQueueMessageCommand(
    string RoutingKey,
    string Body,
    string? CorrelationId,
    string MessageId) : IRequest<ApiResponse<object>>;
