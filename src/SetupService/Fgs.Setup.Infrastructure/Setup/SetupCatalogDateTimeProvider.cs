using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Setup.Application.Abstractions.Time;

namespace Fgs.Setup.Infrastructure.Setup;

public sealed class SetupCatalogDateTimeProvider(IDateTimeProvider dateTimeProvider) : ICatalogDateTimeProvider
{
    public DateTimeOffset UtcNow => dateTimeProvider.UtcNow;
}
