using Fgs.Crm.Application.Abstractions.Time;

namespace Fgs.Crm.Infrastructure.Common.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
