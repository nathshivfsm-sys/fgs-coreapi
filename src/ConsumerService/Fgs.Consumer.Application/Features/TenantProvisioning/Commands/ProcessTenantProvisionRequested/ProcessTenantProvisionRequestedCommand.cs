using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Consumer;
using MediatR;

namespace Fgs.Consumer.Application.Features.TenantProvisioning.Commands.ProcessTenantProvisionRequested;

public sealed record ProcessTenantProvisionRequestedCommand(
    TenantProvisionRequestedEvent Event,
    ConsumerMessageContext Context) : IRequest;
