namespace Fgs.Scheduling.Application.Features.Appointments.Dtos;

public sealed record FgsAppointmentSummaryDto(
    long Id,
    short SourceTypeId,
    long SourceId,
    long? CrewId,
    string? CustomerContactName,
    DateOnly ServiceDate,
    TimeOnly ScheduledTime,
    decimal EstimatedHours,
    short AppointmentStatusId);

public sealed record FgsAppointmentDetailDto(
    long Id,
    short SourceTypeId,
    long SourceId,
    long? CrewId,
    string? CustomerContactName,
    DateOnly ServiceDate,
    TimeOnly ScheduledTime,
    decimal EstimatedHours,
    short AppointmentStatusId,
    DateTimeOffset? CustomerApprovedOn);

public sealed record FgsAppointmentCreateDto(
    short SourceTypeId,
    long SourceId,
    long? CrewId,
    string? CustomerContactName,
    DateOnly ServiceDate,
    TimeOnly ScheduledTime,
    decimal EstimatedHours,
    short AppointmentStatusId,
    DateTimeOffset? CustomerApprovedOn);

public sealed record FgsAppointmentListFilters(
    DateOnly? ServiceDate = null,
    short? AppointmentStatusId = null,
    short? SourceTypeId = null,
    long? SourceId = null,
    long? CrewId = null);
