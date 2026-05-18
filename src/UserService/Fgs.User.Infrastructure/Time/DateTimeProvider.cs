using Fgs.User.Application.Abstractions.Time;

namespace Fgs.User.Infrastructure.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
