namespace Fgs.Foundation.Time;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
