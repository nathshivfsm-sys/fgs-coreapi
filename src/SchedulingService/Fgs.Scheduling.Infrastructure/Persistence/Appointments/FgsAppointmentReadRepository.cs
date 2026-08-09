using Fgs.Foundation.Paging;
using Fgs.Scheduling.Application.Abstractions.Appointments;
using Fgs.Scheduling.Application.Common.SchedulingCrud;
using Fgs.Scheduling.Application.Features.Appointments.Dtos;
using Fgs.Scheduling.Domain.Entities;
using Fgs.Scheduling.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Scheduling.Infrastructure.Persistence.Appointments;

internal sealed class FgsAppointmentReadRepository(FgsSchedulingDbContext dbContext) : IFgsAppointmentReadRepository
{
    public async Task<FgsAppointmentDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.FgsAppointments
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity is null ? null : MapToDetail(entity);
    }

    public async Task<PagedResult<FgsAppointmentSummaryDto>> ListAsync(
        SchedulingListQuery query,
        FgsAppointmentListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);

        var dbQuery = dbContext.FgsAppointments.AsNoTracking();

        if (filters.ServiceDate.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.ServiceDate == filters.ServiceDate.Value);
        }

        if (filters.AppointmentStatusId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.AppointmentStatusId == filters.AppointmentStatusId.Value);
        }

        if (filters.SourceTypeId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.SourceTypeId == filters.SourceTypeId.Value);
        }

        if (filters.SourceId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.SourceId == filters.SourceId.Value);
        }

        if (filters.CrewId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.CrewId == filters.CrewId.Value);
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            var pattern = $"%{paging.Search.Trim()}%";
            dbQuery = dbQuery.Where(e =>
                e.CustomerContactName != null && EF.Functions.ILike(e.CustomerContactName, pattern));
        }

        dbQuery = ApplySort(dbQuery, paging.SortBy, paging.SortDirection);

        var totalCount = await dbQuery.CountAsync(cancellationToken);
        var items = await dbQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new FgsAppointmentSummaryDto(
                e.Id,
                e.SourceTypeId,
                e.SourceId,
                e.CrewId,
                e.CustomerContactName,
                e.ServiceDate,
                e.ScheduledTime,
                e.EstimatedHours,
                e.AppointmentStatusId))
            .ToListAsync(cancellationToken);

        return new PagedResult<FgsAppointmentSummaryDto>(items, page, pageSize, totalCount);
    }

    private static IQueryable<FgsAppointment> ApplySort(
        IQueryable<FgsAppointment> query,
        string? sortBy,
        SortDirection direction)
    {
        var desc = direction == SortDirection.Desc;
        return (sortBy?.Trim().ToLowerInvariant()) switch
        {
            "servicedate" => desc ? query.OrderByDescending(e => e.ServiceDate) : query.OrderBy(e => e.ServiceDate),
            "scheduledtime" => desc ? query.OrderByDescending(e => e.ScheduledTime) : query.OrderBy(e => e.ScheduledTime),
            "appointmentstatusid" => desc
                ? query.OrderByDescending(e => e.AppointmentStatusId)
                : query.OrderBy(e => e.AppointmentStatusId),
            "sourcetypeid" => desc ? query.OrderByDescending(e => e.SourceTypeId) : query.OrderBy(e => e.SourceTypeId),
            "createdon" => desc ? query.OrderByDescending(e => e.CreatedOn) : query.OrderBy(e => e.CreatedOn),
            _ => desc ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id)
        };
    }

    internal static FgsAppointmentDetailDto MapToDetail(FgsAppointment entity) =>
        new(
            entity.Id,
            entity.SourceTypeId,
            entity.SourceId,
            entity.CrewId,
            entity.CustomerContactName,
            entity.ServiceDate,
            entity.ScheduledTime,
            entity.EstimatedHours,
            entity.AppointmentStatusId,
            entity.CustomerApprovedOn);
}
