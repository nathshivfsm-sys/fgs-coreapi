using Fgs.Scheduling.Application.Abstractions.Time;

namespace Fgs.Scheduling.Infrastructure.Common.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
