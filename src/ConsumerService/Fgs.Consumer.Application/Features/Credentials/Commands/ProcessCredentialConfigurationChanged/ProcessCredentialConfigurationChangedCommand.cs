using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Consumer;
using MediatR;

namespace Fgs.Consumer.Application.Features.Credentials.Commands.ProcessCredentialConfigurationChanged;

public sealed record ProcessCredentialConfigurationChangedCommand(
    CredentialConfigurationChangedEvent Event,
    ConsumerMessageContext Context) : IRequest;
