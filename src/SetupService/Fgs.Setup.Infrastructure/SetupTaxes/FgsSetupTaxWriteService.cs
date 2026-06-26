using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.SetupTaxes;

public sealed class FgsSetupTaxWriteService : IFgsSetupTaxWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSetupTaxWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSetupTaxDetailDto> CreateAsync(
        FgsSetupTaxCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupTax
        {
            TaxCode = NormalizeCode(dto.TaxCode),
            Name = dto.Name.Trim(),
            IsExternalSystemRecord = dto.IsExternalSystemRecord,
            ExternalSystemId = string.IsNullOrWhiteSpace(dto.ExternalSystemId) ? null : dto.ExternalSystemId.Trim(),
            SyncToken = string.IsNullOrWhiteSpace(dto.SyncToken) ? null : dto.SyncToken.Trim(),
            ShowTaxDetail = dto.ShowTaxDetail,
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim()
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupTaxes.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTaxDetailDto> UpdateAsync(
        long id,
        FgsSetupTaxUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax '{id}' was not found.");

        entity.TaxCode = NormalizeCode(dto.TaxCode);
        entity.Name = dto.Name.Trim();
        entity.IsExternalSystemRecord = dto.IsExternalSystemRecord;
        entity.ExternalSystemId = string.IsNullOrWhiteSpace(dto.ExternalSystemId) ? null : dto.ExternalSystemId.Trim();
        entity.SyncToken = string.IsNullOrWhiteSpace(dto.SyncToken) ? null : dto.SyncToken.Trim();
        entity.ShowTaxDetail = dto.ShowTaxDetail;
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTaxDetailDto> PatchAsync(
        long id,
        FgsSetupTaxPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax '{id}' was not found.");

        if (dto.TaxCode is not null)
        {
            entity.TaxCode = NormalizeCode(dto.TaxCode); ;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim(); ;
        }
        if (dto.IsExternalSystemRecord.HasValue)
        {
            entity.IsExternalSystemRecord = dto.IsExternalSystemRecord.Value;
        }
        if (dto.ExternalSystemId is not null)
        {
            entity.ExternalSystemId = string.IsNullOrWhiteSpace(dto.ExternalSystemId) ? null : dto.ExternalSystemId.Trim(); ;
        }
        if (dto.SyncToken is not null)
        {
            entity.SyncToken = string.IsNullOrWhiteSpace(dto.SyncToken) ? null : dto.SyncToken.Trim(); ;
        }
        if (dto.ShowTaxDetail.HasValue)
        {
            entity.ShowTaxDetail = dto.ShowTaxDetail.Value;
        }
        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(); ;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTaxDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupTax?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupTaxes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A tax with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupTaxDetailDto MapToDetail(FgsSetupTax entity) =>
        new(
            entity.Id,
            entity.TaxCode,
            entity.Name,
            entity.IsExternalSystemRecord,
            entity.ExternalSystemId,
            entity.SyncToken,
            entity.ShowTaxDetail,
            entity.Description,
            entity.IsActive);
}
