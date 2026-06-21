using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;

namespace Fgs.Setup.Infrastructure.SetupTimeSlots;

internal sealed class FgsSetupTimeSlotSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long? FgsSetupZoneId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public TimeSpan BeginTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public TimeSpan? MarkTechArrivedLateAfter { get; set; }
    public TimeSpan? MarkWorkOrderDelayedCompletionAfter { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsCustomerPortalVisible { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSetupTimeSlotSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            FgsSetupZoneId,
            Code,
            Name,
            BeginTime,
            EndTime,
            MarkTechArrivedLateAfter,
            MarkWorkOrderDelayedCompletionAfter,
            IsMobileVisible,
            IsCustomerPortalVisible,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSetupTimeSlotDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long? FgsSetupZoneId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public TimeSpan BeginTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public TimeSpan? MarkTechArrivedLateAfter { get; set; }
    public TimeSpan? MarkWorkOrderDelayedCompletionAfter { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsCustomerPortalVisible { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSetupTimeSlotDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            FgsSetupZoneId,
            Code,
            Name,
            BeginTime,
            EndTime,
            MarkTechArrivedLateAfter,
            MarkWorkOrderDelayedCompletionAfter,
            IsMobileVisible,
            IsCustomerPortalVisible,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSetupTimeSlotLookupRow
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }

    public FgsSetupTimeSlotLookupDto ToDto() => new(Id,
            Code,
            Name);
}
