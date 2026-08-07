using Fgs.Billing.Application.Abstractions.Time;

namespace Fgs.Billing.Infrastructure.Common.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
