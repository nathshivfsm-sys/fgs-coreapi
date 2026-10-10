using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Consumer;
using MediatR;

namespace Fgs.Consumer.Application.Features.Notifications.Commands.ProcessUserInvited;

public sealed record ProcessUserInvitedCommand(
    UserInvitedEvent Event,
    ConsumerMessageContext Context) : IRequest;
