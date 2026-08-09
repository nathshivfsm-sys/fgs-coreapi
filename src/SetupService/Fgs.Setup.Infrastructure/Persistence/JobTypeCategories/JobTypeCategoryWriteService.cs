using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeCategories;

public sealed class JobTypeCategoryWriteService : IJobTypeCategoryWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public JobTypeCategoryWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<JobTypeCategoryDetailDto> CreateAsync(
        JobTypeCategoryCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsJobTypeCategory
        {
            JobTypeId = dto.JobTypeId, JobCategoryId = dto.JobCategoryId, DisplayOrder = dto.DisplayOrder ?? 1
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsJobTypeCategories.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeCategoryDetailDto> UpdateAsync(
        long id,
        JobTypeCategoryUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type Category '{id}' was not found.");

        entity.JobTypeId = dto.JobTypeId;
        entity.JobCategoryId = dto.JobCategoryId;
        entity.DisplayOrder = dto.DisplayOrder ?? entity.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeCategoryDetailDto> PatchAsync(
        long id,
        JobTypeCategoryPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type Category '{id}' was not found.");

        if (dto.JobTypeId.HasValue)
        {
            entity.JobTypeId = dto.JobTypeId.Value;
        }
        if (dto.JobCategoryId.HasValue)
        {
            entity.JobCategoryId = dto.JobCategoryId.Value;
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

    public async Task<JobTypeCategoryDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type Category '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsJobTypeCategory?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsJobTypeCategories.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A job type category with the same type and name already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static JobTypeCategoryDetailDto MapToDetail(FgsJobTypeCategory entity) =>
        new(
            entity.Id,
            entity.JobTypeId,
            entity.JobCategoryId,
            entity.DisplayOrder,
            entity.IsActive);
}
