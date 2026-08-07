using Fgs.ServiceAgreement.Application.Abstractions.Time;

namespace Fgs.ServiceAgreement.Infrastructure.Common.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
