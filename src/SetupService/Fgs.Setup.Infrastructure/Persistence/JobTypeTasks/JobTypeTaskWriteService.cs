using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeTasks;

public sealed class JobTypeTaskWriteService : IJobTypeTaskWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public JobTypeTaskWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<JobTypeTaskDetailDto> CreateAsync(
        JobTypeTaskCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsJobTypeTask
        {
            JobTypeCategoryId = dto.JobTypeCategoryId, TradeId = dto.TradeId, TaskName = dto.TaskName.Trim(), Priority = dto.Priority, EstimatedHours = dto.EstimatedHours, DisplayOrder = dto.DisplayOrder ?? 1
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsJobTypeTasks.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeTaskDetailDto> UpdateAsync(
        long id,
        JobTypeTaskUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type Task '{id}' was not found.");

        entity.JobTypeCategoryId = dto.JobTypeCategoryId;
        entity.TradeId = dto.TradeId;
        entity.TaskName = dto.TaskName.Trim();
        entity.Priority = dto.Priority;
        entity.EstimatedHours = dto.EstimatedHours;
        entity.DisplayOrder = dto.DisplayOrder ?? entity.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeTaskDetailDto> PatchAsync(
        long id,
        JobTypeTaskPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type Task '{id}' was not found.");

        if (dto.JobTypeCategoryId.HasValue)
        {
            entity.JobTypeCategoryId = dto.JobTypeCategoryId.Value;
        }
        if (dto.TradeId.HasValue)
        {
            entity.TradeId = dto.TradeId.Value;
        }
        if (dto.TaskName is not null)
        {
            entity.TaskName = dto.TaskName.Trim();;
        }
        if (dto.Priority.HasValue)
        {
            entity.Priority = dto.Priority.Value;
        }
        if (dto.EstimatedHours.HasValue)
        {
            entity.EstimatedHours = dto.EstimatedHours.Value;
        }
        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeTaskDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type Task '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsJobTypeTask?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsJobTypeTasks.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A job type task with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static JobTypeTaskDetailDto MapToDetail(FgsJobTypeTask entity) =>
        new(
            entity.Id,
            entity.JobTypeCategoryId,
            entity.TradeId,
            entity.TaskName,
            entity.Priority,
            entity.EstimatedHours,
            entity.DisplayOrder,
            entity.IsActive);
}
