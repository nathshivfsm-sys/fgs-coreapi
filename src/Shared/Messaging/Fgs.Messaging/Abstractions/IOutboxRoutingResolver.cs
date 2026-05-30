using Fgs.Messaging.Models;

namespace Fgs.Messaging.Abstractions;

public interface IOutboxRoutingResolver
{
    string ResolveRoutingKey(PendingOutboxMessage message);

    string ResolveExchangeName(PendingOutboxMessage message);
}
