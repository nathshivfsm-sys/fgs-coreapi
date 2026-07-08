using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Consumer;
using MediatR;

namespace Fgs.Consumer.Application.Features.Audit.Commands.ProcessCredentialAuditRequested;

public sealed record ProcessCredentialAuditRequestedCommand(
    CredentialAuditRequestedEvent Event,
    ConsumerMessageContext Context) : IRequest;
