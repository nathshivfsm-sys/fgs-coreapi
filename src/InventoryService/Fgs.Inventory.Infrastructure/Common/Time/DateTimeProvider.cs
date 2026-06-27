using Fgs.Inventory.Application.Abstractions.Time;

namespace Fgs.Inventory.Infrastructure.Common.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
