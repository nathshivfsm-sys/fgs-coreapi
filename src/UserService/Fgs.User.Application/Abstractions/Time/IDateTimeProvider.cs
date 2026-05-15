namespace Fgs.User.Application.Abstractions.Time;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
