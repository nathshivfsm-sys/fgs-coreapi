using Fgs.Messaging.Models;

namespace Fgs.Messaging.Abstractions;

public interface IOutboxDestinationResolver
{
    IntegrationEventDestination Resolve(PendingOutboxMessage message);
}
