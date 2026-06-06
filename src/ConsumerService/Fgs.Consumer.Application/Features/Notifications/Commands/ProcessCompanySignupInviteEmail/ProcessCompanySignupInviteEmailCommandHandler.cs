using System.Text.Json;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Clients;
using Fgs.Contracts.Requests;
using Fgs.Messaging.Serialization;
using MediatR;

namespace Fgs.Consumer.Application.Features.Notifications.Commands.ProcessCompanySignupInviteEmail;

public sealed class ProcessCompanySignupInviteEmailCommandHandler(INotificationDispatchClient notificationClient)
    : IRequestHandler<ProcessCompanySignupInviteEmailCommand>
{
    private static readonly JsonSerializerOptions JsonOptions = IntegrationEventJsonSerializerOptions.Create();

    public async Task Handle(
        ProcessCompanySignupInviteEmailCommand request,
        CancellationToken cancellationToken)
    {
        var context = request.Context;
        var dispatchRequest = new DispatchNotificationRequest
        {
            RoutingKey = IntegrationEventRoutingKeys.CompanySignupInviteEmail,
            Payload = JsonSerializer.Serialize(request.Event, JsonOptions),
            CorrelationId = context.CorrelationId,
            MessageId = context.MessageId
        };

        var response = await notificationClient.DispatchAsync(dispatchRequest, cancellationToken);
        if (!response.Success)
        {
            var message = response.Errors.Count > 0
                ? string.Join("; ", response.Errors)
                : "Notification dispatch failed.";
            throw new InvalidOperationException(message);
        }
    }
}
