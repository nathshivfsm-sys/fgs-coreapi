using Fgs.Notification.Application.Integrations.QuickBooks;

namespace Fgs.Notification.Infrastructure.Integrations.QuickBooks;

public sealed class QuickBooksIntegrationClient : IQuickBooksIntegrationClient
{
    public string IntegrationName => "QuickBooks";
}
