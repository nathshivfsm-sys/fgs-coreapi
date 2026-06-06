using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Consumer;
using MediatR;

namespace Fgs.Consumer.Application.Features.Notifications.Commands.ProcessCompanySignupInviteEmail;

public sealed record ProcessCompanySignupInviteEmailCommand(
    CompanySignupInviteEmailEvent Event,
    ConsumerMessageContext Context) : IRequest;
