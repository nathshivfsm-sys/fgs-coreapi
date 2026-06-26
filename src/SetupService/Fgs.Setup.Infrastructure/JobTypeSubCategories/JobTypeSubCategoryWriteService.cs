using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.JobTypeSubCategories;

public sealed class JobTypeSubCategoryWriteService : IJobTypeSubCategoryWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public JobTypeSubCategoryWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<JobTypeSubCategoryDetailDto> CreateAsync(
        JobTypeSubCategoryCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsJobTypeSubCategory
        {
            SubCategoryCode = NormalizeCode(dto.SubCategoryCode),
            Name = dto.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            DisplayOrder = dto.DisplayOrder ?? 1
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsJobTypeSubCategories.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeSubCategoryDetailDto> UpdateAsync(
        long id,
        JobTypeSubCategoryUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type Subcategory '{id}' was not found.");

        entity.SubCategoryCode = NormalizeCode(dto.SubCategoryCode);
        entity.Name = dto.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.DisplayOrder = dto.DisplayOrder ?? entity.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeSubCategoryDetailDto> PatchAsync(
        long id,
        JobTypeSubCategoryPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type Subcategory '{id}' was not found.");

        if (dto.SubCategoryCode is not null)
        {
            entity.SubCategoryCode = NormalizeCode(dto.SubCategoryCode); ;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim(); ;
        }
        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(); ;
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

    public async Task<JobTypeSubCategoryDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type Subcategory '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsJobTypeSubCategory?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsJobTypeSubCategories.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A job type subcategory with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static JobTypeSubCategoryDetailDto MapToDetail(FgsJobTypeSubCategory entity) =>
        new(
            entity.Id,
            entity.SubCategoryCode,
            entity.Name,
            entity.Description,
            entity.DisplayOrder,
            entity.IsActive);
}
