using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.JobTypes;

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
            JobTypeCategoryId = dto.JobTypeCategoryId, JobTypeSubCategoryId = dto.JobTypeSubCategoryId, JobTypeCode = NormalizeCode(dto.JobTypeCode), TaskName = dto.TaskName.Trim(), Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(), UsedFor = dto.UsedFor.Trim(), Trade = string.IsNullOrWhiteSpace(dto.Trade) ? null : dto.Trade.Trim(), EstimatedDurationMinutes = dto.EstimatedDurationMinutes ?? 1, BusinessUnit = string.IsNullOrWhiteSpace(dto.BusinessUnit) ? null : dto.BusinessUnit.Trim(), Priority = dto.Priority, BackgroundColor = string.IsNullOrWhiteSpace(dto.BackgroundColor) ? null : dto.BackgroundColor.Trim(), TextColor = string.IsNullOrWhiteSpace(dto.TextColor) ? null : dto.TextColor.Trim(), ShowToFieldTech = dto.ShowToFieldTech, ShowOnCustomerPortal = dto.ShowOnCustomerPortal, DisplayOrder = dto.DisplayOrder ?? 1
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsJobTypes.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeDetailDto> UpdateAsync(
        long id,
        JobTypeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type '{id}' was not found.");

        entity.JobTypeCategoryId = dto.JobTypeCategoryId;
        entity.JobTypeSubCategoryId = dto.JobTypeSubCategoryId;
        entity.JobTypeCode = NormalizeCode(dto.JobTypeCode);
        entity.TaskName = dto.TaskName.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.UsedFor = dto.UsedFor.Trim();
        entity.Trade = string.IsNullOrWhiteSpace(dto.Trade) ? null : dto.Trade.Trim();
        entity.EstimatedDurationMinutes = dto.EstimatedDurationMinutes ?? entity.EstimatedDurationMinutes;
        entity.BusinessUnit = string.IsNullOrWhiteSpace(dto.BusinessUnit) ? null : dto.BusinessUnit.Trim();
        entity.Priority = dto.Priority;
        entity.BackgroundColor = string.IsNullOrWhiteSpace(dto.BackgroundColor) ? null : dto.BackgroundColor.Trim();
        entity.TextColor = string.IsNullOrWhiteSpace(dto.TextColor) ? null : dto.TextColor.Trim();
        entity.ShowToFieldTech = dto.ShowToFieldTech;
        entity.ShowOnCustomerPortal = dto.ShowOnCustomerPortal;
        entity.DisplayOrder = dto.DisplayOrder ?? entity.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeDetailDto> PatchAsync(
        long id,
        JobTypePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type '{id}' was not found.");

        if (dto.JobTypeCategoryId.HasValue)
        {
            entity.JobTypeCategoryId = dto.JobTypeCategoryId.Value;
        }
        if (dto.JobTypeSubCategoryId.HasValue)
        {
            entity.JobTypeSubCategoryId = dto.JobTypeSubCategoryId.Value;
        }
        if (dto.JobTypeCode is not null)
        {
            entity.JobTypeCode = NormalizeCode(dto.JobTypeCode);;
        }
        if (dto.TaskName is not null)
        {
            entity.TaskName = dto.TaskName.Trim();;
        }
        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();;
        }
        if (dto.UsedFor is not null)
        {
            entity.UsedFor = dto.UsedFor.Trim();;
        }
        if (dto.Trade is not null)
        {
            entity.Trade = string.IsNullOrWhiteSpace(dto.Trade) ? null : dto.Trade.Trim();;
        }
        if (dto.EstimatedDurationMinutes.HasValue)
        {
            entity.EstimatedDurationMinutes = dto.EstimatedDurationMinutes.Value;
        }
        if (dto.BusinessUnit is not null)
        {
            entity.BusinessUnit = string.IsNullOrWhiteSpace(dto.BusinessUnit) ? null : dto.BusinessUnit.Trim();;
        }
        if (dto.Priority.HasValue)
        {
            entity.Priority = dto.Priority.Value;
        }
        if (dto.BackgroundColor is not null)
        {
            entity.BackgroundColor = string.IsNullOrWhiteSpace(dto.BackgroundColor) ? null : dto.BackgroundColor.Trim();;
        }
        if (dto.TextColor is not null)
        {
            entity.TextColor = string.IsNullOrWhiteSpace(dto.TextColor) ? null : dto.TextColor.Trim();;
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

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<JobTypeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Job Type '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsJobType?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsJobTypes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A job type with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static JobTypeDetailDto MapToDetail(FgsJobType entity) =>
        new(
            entity.Id,
            entity.TenantId,
            entity.CompanyId,
            entity.JobTypeCategoryId,
            entity.JobTypeSubCategoryId,
            entity.JobTypeCode,
            entity.TaskName,
            entity.Description,
            entity.UsedFor,
            entity.Trade,
            entity.EstimatedDurationMinutes,
            entity.BusinessUnit,
            entity.Priority,
            entity.BackgroundColor,
            entity.TextColor,
            entity.ShowToFieldTech,
            entity.ShowOnCustomerPortal,
            entity.DisplayOrder,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy,
            entity.UpdatedOn,
            entity.UpdatedBy);
}
