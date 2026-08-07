using Fgs.Persistence.Abstractions;
using Fgs.Scheduling.Application.Abstractions.Appointments;
using Fgs.Scheduling.Application.Features.Appointments.Dtos;
using Fgs.Scheduling.Domain.Entities;
using Fgs.Scheduling.Infrastructure.Common;
using Fgs.Scheduling.Infrastructure.Database;

namespace Fgs.Scheduling.Infrastructure.Persistence.Appointments;

public sealed class FgsAppointmentWriteService : IFgsAppointmentWriteService
{
    private readonly FgsSchedulingDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SchedulingEntityAuditHelper _auditHelper;

    public FgsAppointmentWriteService(
        FgsSchedulingDbContext context,
        IUnitOfWork unitOfWork,
        SchedulingEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsAppointmentDetailDto> CreateAsync(
        FgsAppointmentCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsAppointment
        {
            SourceTypeId = dto.SourceTypeId,
            SourceId = dto.SourceId,
            CrewId = dto.CrewId,
            CustomerContactName = TrimOrNull(dto.CustomerContactName),
            ServiceDate = dto.ServiceDate,
            ScheduledTime = dto.ScheduledTime,
            EstimatedHours = dto.EstimatedHours,
            AppointmentStatusId = dto.AppointmentStatusId,
            CustomerApprovedOn = dto.CustomerApprovedOn
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsAppointments.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return FgsAppointmentReadRepository.MapToDetail(entity);
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
