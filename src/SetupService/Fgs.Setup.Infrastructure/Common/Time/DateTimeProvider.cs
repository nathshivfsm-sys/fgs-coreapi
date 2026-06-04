using Fgs.Setup.Application.Abstractions.Time;

namespace Fgs.Setup.Infrastructure.Common.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
