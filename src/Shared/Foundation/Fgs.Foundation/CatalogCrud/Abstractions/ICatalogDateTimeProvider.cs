namespace Fgs.Foundation.CatalogCrud.Abstractions;

public interface ICatalogDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
