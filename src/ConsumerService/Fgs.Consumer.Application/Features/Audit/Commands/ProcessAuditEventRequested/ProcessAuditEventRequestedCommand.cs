using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Consumer;
using MediatR;

namespace Fgs.Consumer.Application.Features.Audit.Commands.ProcessAuditEventRequested;

public sealed record ProcessAuditEventRequestedCommand(
    AuditEventRequestedEvent Event,
    ConsumerMessageContext Context) : IRequest;
