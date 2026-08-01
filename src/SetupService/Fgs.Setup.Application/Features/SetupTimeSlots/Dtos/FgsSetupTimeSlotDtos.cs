namespace Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;

public sealed record FgsSetupTimeSlotSummaryDto(
    long Id,
    long? FgsSetupZoneId,
    string Code,
    string Name,
    TimeSpan BeginTime,
    TimeSpan EndTime,
    TimeSpan? MarkTechArrivedLateAfter,
    TimeSpan? MarkWorkOrderDelayedCompletionAfter,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible,
    bool IncludeInCapacityPlanning,
    bool ShowToExternalSystem,
    bool IsActive);

public sealed record FgsSetupTimeSlotDetailDto(
    long Id,
    long? FgsSetupZoneId,
    string Code,
    string Name,
    TimeSpan BeginTime,
    TimeSpan EndTime,
    TimeSpan? MarkTechArrivedLateAfter,
    TimeSpan? MarkWorkOrderDelayedCompletionAfter,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible,
    bool IncludeInCapacityPlanning,
    bool ShowToExternalSystem,
    bool IsActive);

public sealed record FgsSetupTimeSlotLookupDto(
    long Id,
    string Code,
    string Name);

public sealed record FgsSetupTimeSlotCreateDto(
    long? FgsSetupZoneId,
    string Code,
    string Name,
    TimeSpan BeginTime,
    TimeSpan EndTime,
    TimeSpan? MarkTechArrivedLateAfter,
    TimeSpan? MarkWorkOrderDelayedCompletionAfter,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible,
    bool IncludeInCapacityPlanning = false,
    bool ShowToExternalSystem = false);

public sealed record FgsSetupTimeSlotUpdateDto(
    long? FgsSetupZoneId,
    string Code,
    string Name,
    TimeSpan BeginTime,
    TimeSpan EndTime,
    TimeSpan? MarkTechArrivedLateAfter,
    TimeSpan? MarkWorkOrderDelayedCompletionAfter,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible,
    bool IncludeInCapacityPlanning = false,
    bool ShowToExternalSystem = false);

public sealed record FgsSetupTimeSlotPatchDto(
    long? FgsSetupZoneId,
    string? Code,
    string? Name,
    TimeSpan? BeginTime,
    TimeSpan? EndTime,
    TimeSpan? MarkTechArrivedLateAfter,
    TimeSpan? MarkWorkOrderDelayedCompletionAfter,
    bool? IsMobileVisible,
    bool? IsCustomerPortalVisible,
    bool? IncludeInCapacityPlanning,
    bool? ShowToExternalSystem,
    bool? IsActive);

public sealed record FgsSetupTimeSlotListFilters(
    string? Code = null,
    string? Name = null);
