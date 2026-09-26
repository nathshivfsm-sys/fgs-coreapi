using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
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
        var name = dto.Name.Trim();
        var categoryName = await GetCategoryNameAsync(dto.JobTypeCategoryId, cancellationToken);
        var taskName = ResolveTaskName(dto.TaskName, categoryName, name);

        var entity = new FgsJobTypeTask
        {
            JobTypeCategoryId = dto.JobTypeCategoryId,
            TradeId = dto.TradeId,
            SkillLevelId = dto.SkillLevelId,
            Name = name,
            TaskName = taskName,
            Priority = dto.Priority,
            EstimatedHours = dto.EstimatedHours,
            DisplayOrder = dto.DisplayOrder ?? 1
        };

        _auditHelper.StampForCreate(entity);
        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        await _context.FgsJobTypeTasks.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity, categoryName);
    }

    public async Task<JobTypeTaskDetailDto> UpdateAsync(
        long id,
        JobTypeTaskUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type Task '{id}' was not found.");

        var name = dto.Name.Trim();
        var categoryName = await GetCategoryNameAsync(dto.JobTypeCategoryId, cancellationToken);

        entity.JobTypeCategoryId = dto.JobTypeCategoryId;
        entity.TradeId = dto.TradeId;
        entity.SkillLevelId = dto.SkillLevelId;
        entity.Name = name;
        entity.TaskName = ResolveTaskName(dto.TaskName, categoryName, name);
        entity.Priority = dto.Priority;
        entity.EstimatedHours = dto.EstimatedHours;
        entity.DisplayOrder = dto.DisplayOrder ?? entity.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity, categoryName);
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
        if (dto.SkillLevelId.HasValue)
        {
            entity.SkillLevelId = dto.SkillLevelId.Value;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
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

        var categoryName = await GetCategoryNameAsync(entity.JobTypeCategoryId, cancellationToken);
        if (dto.TaskName is not null)
        {
            entity.TaskName = dto.TaskName.Trim();
        }
        else if (dto.Name is not null || dto.JobTypeCategoryId.HasValue)
        {
            entity.TaskName = ComposeTaskName(categoryName, entity.Name);
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity, categoryName);
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
        await _context.FgsJobTypeTasks.FirstOrDefaultIncludingInactiveAsync(e => e.Id == id, cancellationToken);

    private async Task<string?> GetCategoryNameAsync(long jobTypeCategoryId, CancellationToken cancellationToken)
    {
        return await (
            from jobTypeCategory in _context.FgsJobTypeCategories.AsNoTracking()
            join jobCategory in _context.FgsJobCategories.AsNoTracking()
                on jobTypeCategory.JobCategoryId equals jobCategory.Id
            where jobTypeCategory.Id == jobTypeCategoryId
            select jobCategory.Name).FirstOrDefaultAsync(cancellationToken);
    }

    internal static string ResolveTaskName(string? taskName, string? categoryName, string subCategoryName)
    {
        if (!string.IsNullOrWhiteSpace(taskName))
        {
            return taskName.Trim();
        }

        return ComposeTaskName(categoryName, subCategoryName);
    }

    internal static string ComposeTaskName(string? categoryName, string subCategoryName)
    {
        var name = subCategoryName.Trim();
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            return name;
        }

        return $"{categoryName.Trim()} {name}";
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A sub-category with this name already exists in the selected category.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static JobTypeTaskDetailDto MapToDetail(FgsJobTypeTask entity, string? categoryName = null) =>
        new(
            entity.Id,
            entity.JobTypeCategoryId,
            entity.TradeId,
            entity.SkillLevelId,
            entity.Name,
            entity.TaskName,
            entity.Priority,
            entity.EstimatedHours,
            entity.DisplayOrder,
            entity.IsActive,
            categoryName);
}
