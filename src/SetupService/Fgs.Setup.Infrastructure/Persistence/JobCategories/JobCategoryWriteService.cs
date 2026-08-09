using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.JobCategories;

public sealed class JobCategoryWriteService : IJobCategoryWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public JobCategoryWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<JobCategoryDetailDto> CreateAsync(
        JobCategoryCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsJobCategory
        {
            CategoryCode = NormalizeCode(dto.CategoryCode), Name = dto.Name.Trim(), DisplayOrder = dto.DisplayOrder ?? 1
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsJobCategories.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobCategoryDetailDto> UpdateAsync(
        long id,
        JobCategoryUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Category '{id}' was not found.");

        entity.CategoryCode = NormalizeCode(dto.CategoryCode);
        entity.Name = dto.Name.Trim();
        entity.DisplayOrder = dto.DisplayOrder ?? entity.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobCategoryDetailDto> PatchAsync(
        long id,
        JobCategoryPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Category '{id}' was not found.");

        if (dto.CategoryCode is not null)
        {
            entity.CategoryCode = NormalizeCode(dto.CategoryCode);;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();;
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

    public async Task<JobCategoryDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Category '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsJobCategory?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsJobCategories.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A job category with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static JobCategoryDetailDto MapToDetail(FgsJobCategory entity) =>
        new(
            entity.Id,
            entity.CategoryCode,
            entity.Name,
            entity.DisplayOrder,
            entity.IsActive);
}
