using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using Fgs.Setup.Domain.Enums;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypes;

public sealed class JobTypeWriteService : IJobTypeWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public JobTypeWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<JobTypeDetailDto> CreateAsync(
        JobTypeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsJobType
        {
            JobTypeCode = NormalizeCode(dto.JobTypeCode),
            Name = dto.Name.Trim(),
            UsedFor = (JobTypeUsedFor)dto.UsedFor,
            BusinessUnit = string.IsNullOrWhiteSpace(dto.BusinessUnit) ? null : dto.BusinessUnit.Trim(),
            ShowToFieldTech = dto.ShowToFieldTech,
            ShowOnCustomerPortal = dto.ShowOnCustomerPortal,
            DisplayOrder = dto.DisplayOrder ?? 1
        };

        _auditHelper.StampForCreate(entity);
        entity.IsActive = dto.IsActive;
        SyncSubCategories(entity, dto.SubCategories ?? []);
        await _context.FgsJobTypes.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeDetailDto> UpdateAsync(
        long id,
        JobTypeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityWithCategoriesAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type '{id}' was not found.");

        entity.JobTypeCode = NormalizeCode(dto.JobTypeCode);
        entity.Name = dto.Name.Trim();
        entity.UsedFor = (JobTypeUsedFor)dto.UsedFor;
        entity.BusinessUnit = string.IsNullOrWhiteSpace(dto.BusinessUnit) ? null : dto.BusinessUnit.Trim();
        entity.ShowToFieldTech = dto.ShowToFieldTech;
        entity.ShowOnCustomerPortal = dto.ShowOnCustomerPortal;
        entity.DisplayOrder = dto.DisplayOrder ?? entity.DisplayOrder;

        SyncSubCategories(entity, dto.SubCategories ?? []);
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeDetailDto> PatchAsync(
        long id,
        JobTypePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityWithCategoriesAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type '{id}' was not found.");

        if (dto.JobTypeCode is not null)
        {
            entity.JobTypeCode = NormalizeCode(dto.JobTypeCode);
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }
        if (dto.UsedFor.HasValue)
        {
            entity.UsedFor = (JobTypeUsedFor)dto.UsedFor.Value;
        }
        if (dto.BusinessUnit is not null)
        {
            entity.BusinessUnit = string.IsNullOrWhiteSpace(dto.BusinessUnit) ? null : dto.BusinessUnit.Trim();
        }
        if (dto.ShowToFieldTech.HasValue)
        {
            entity.ShowToFieldTech = dto.ShowToFieldTech.Value;
        }
        if (dto.ShowOnCustomerPortal.HasValue)
        {
            entity.ShowOnCustomerPortal = dto.ShowOnCustomerPortal.Value;
        }
        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        if (dto.SubCategories is not null)
        {
            SyncSubCategories(entity, dto.SubCategories);
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityWithCategoriesAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private void SyncSubCategories(
        FgsJobType entity,
        IReadOnlyList<JobTypeSubCategoryWriteDto> items)
    {
        var desiredTaskIds = items.Select(item => item.JobTypeTaskId).ToHashSet();

        foreach (var existing in entity.JobTypeCategories.Where(c => !desiredTaskIds.Contains(c.JobTypeTaskId)))
        {
            if (!existing.IsActive)
            {
                continue;
            }

            existing.IsActive = false;
            _auditHelper.StampForUpdate(existing);
        }

        var existingByTaskId = entity.JobTypeCategories
            .GroupBy(c => c.JobTypeTaskId)
            .ToDictionary(g => g.Key, g => g.First());

        short fallbackOrder = 1;
        foreach (var item in items)
        {
            var displayOrder = item.DisplayOrder ?? fallbackOrder;
            fallbackOrder++;

            if (existingByTaskId.TryGetValue(item.JobTypeTaskId, out var existing))
            {
                existing.DisplayOrder = displayOrder;
                existing.IsActive = item.IsActive;
                _auditHelper.StampForUpdate(existing);
                continue;
            }

            var mapping = new FgsJobTypeCategory
            {
                JobTypeId = entity.Id,
                JobTypeTaskId = item.JobTypeTaskId,
                DisplayOrder = displayOrder
            };
            _auditHelper.StampForCreate(mapping);
            mapping.IsActive = item.IsActive;
            entity.JobTypeCategories.Add(mapping);
            _context.FgsJobTypeCategories.Add(mapping);
        }
    }

    private async Task<FgsJobType?> FindEntityWithCategoriesAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsJobTypes
            .Include(e => e.JobTypeCategories)
            .FirstOrDefaultIncludingInactiveAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A job type with the same code or subcategory assignment already exists.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private JobTypeDetailDto MapToDetail(FgsJobType entity)
    {
        var taskIds = entity.JobTypeCategories.Select(c => c.JobTypeTaskId).Distinct().ToArray();
        var tasks = _context.FgsJobTypeTasks
            .AsNoTracking()
            .Where(t => taskIds.Contains(t.Id))
            .Select(t => new { t.Id, t.JobTypeCategoryId, t.Name })
            .ToList()
            .ToDictionary(t => t.Id);
        var categoryIds = tasks.Values.Select(t => t.JobTypeCategoryId).Distinct().ToArray();
        var categoryNames = _context.FgsJobCategories
            .AsNoTracking()
            .Where(c => categoryIds.Contains(c.Id))
            .Select(c => new { c.Id, c.Name })
            .ToList()
            .ToDictionary(c => c.Id, c => c.Name);

        return new(
            entity.Id,
            entity.JobTypeCode,
            entity.Name,
            (short)entity.UsedFor,
            entity.BusinessUnit,
            entity.ShowToFieldTech,
            entity.ShowOnCustomerPortal,
            entity.DisplayOrder,
            entity.IsActive,
            entity.JobTypeCategories
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Id)
                .Select(c =>
                {
                    tasks.TryGetValue(c.JobTypeTaskId, out var task);
                    string? categoryName = null;
                    if (task is not null)
                    {
                        categoryNames.TryGetValue(task.JobTypeCategoryId, out categoryName);
                    }

                    return new JobTypeSubCategoryDto(
                        task?.JobTypeCategoryId ?? 0,
                        categoryName,
                        c.JobTypeTaskId,
                        task?.Name);
                })
                .ToList());
    }
}
