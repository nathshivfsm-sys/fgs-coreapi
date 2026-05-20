using Fgs.User.Application.Abstractions.Time;

namespace Fgs.User.Infrastructure.Common.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
