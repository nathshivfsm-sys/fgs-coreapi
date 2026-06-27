using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;

namespace Fgs.Setup.Infrastructure.SetupTimeSlots;

internal sealed class FgsSetupTimeSlotSummaryRow
{
    public long Id { get; set; }
    public long? FgsSetupZoneId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public TimeSpan BeginTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public TimeSpan? MarkTechArrivedLateAfter { get; set; }
    public TimeSpan? MarkWorkOrderDelayedCompletionAfter { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsCustomerPortalVisible { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupTimeSlotSummaryDto ToDto() =>
        new(
            Id,
            FgsSetupZoneId,
            Code,
            Name,
            BeginTime,
            EndTime,
            MarkTechArrivedLateAfter,
            MarkWorkOrderDelayedCompletionAfter,
            IsMobileVisible,
            IsCustomerPortalVisible,
            IsActive);
}

internal sealed class FgsSetupTimeSlotDetailRow
{
    public long Id { get; set; }
    public long? FgsSetupZoneId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public TimeSpan BeginTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public TimeSpan? MarkTechArrivedLateAfter { get; set; }
    public TimeSpan? MarkWorkOrderDelayedCompletionAfter { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsCustomerPortalVisible { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupTimeSlotDetailDto ToDto() =>
        new(
            Id,
            FgsSetupZoneId,
            Code,
            Name,
            BeginTime,
            EndTime,
            MarkTechArrivedLateAfter,
            MarkWorkOrderDelayedCompletionAfter,
            IsMobileVisible,
            IsCustomerPortalVisible,
            IsActive);
}

internal sealed class FgsSetupTimeSlotLookupRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsSetupTimeSlotLookupDto ToDto() => new(Id,
            Code,
            Name);
}
