using Fgs.Asset.Application.Abstractions.Time;

namespace Fgs.Asset.Infrastructure.Common.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
