namespace Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;

public sealed record FgsSetupTimeSlotSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    long? FgsSetupZoneId,
    string Code,
    string Name,
    TimeSpan BeginTime,
    TimeSpan EndTime,
    TimeSpan? MarkTechArrivedLateAfter,
    TimeSpan? MarkWorkOrderDelayedCompletionAfter,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSetupTimeSlotDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    long? FgsSetupZoneId,
    string Code,
    string Name,
    TimeSpan BeginTime,
    TimeSpan EndTime,
    TimeSpan? MarkTechArrivedLateAfter,
    TimeSpan? MarkWorkOrderDelayedCompletionAfter,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

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
    bool IsCustomerPortalVisible);

public sealed record FgsSetupTimeSlotUpdateDto(
    long? FgsSetupZoneId,
    string Code,
    string Name,
    TimeSpan BeginTime,
    TimeSpan EndTime,
    TimeSpan? MarkTechArrivedLateAfter,
    TimeSpan? MarkWorkOrderDelayedCompletionAfter,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible);

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
    bool? IsActive);

public sealed record FgsSetupTimeSlotListFilters(
    string? Code = null,
    string? Name = null);
