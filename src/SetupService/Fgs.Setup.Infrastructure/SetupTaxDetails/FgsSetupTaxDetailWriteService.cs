using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.SetupTaxDetails;

public sealed class FgsSetupTaxDetailWriteService : IFgsSetupTaxDetailWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSetupTaxDetailWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSetupTaxDetailDetailDto> CreateAsync(
        FgsSetupTaxDetailCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupTaxDetail
        {
            FgsSetupTaxId = dto.FgsSetupTaxId, FgsSetupTaxAuthorityId = dto.FgsSetupTaxAuthorityId, EffectiveFromDate = dto.EffectiveFromDate, EffectiveToDate = dto.EffectiveToDate, TaxPercent = dto.TaxPercent, IsExternalSystemRecord = dto.IsExternalSystemRecord
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupTaxDetails.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTaxDetailDetailDto> UpdateAsync(
        long id,
        FgsSetupTaxDetailUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax Detail '{id}' was not found.");

        entity.FgsSetupTaxId = dto.FgsSetupTaxId;
        entity.FgsSetupTaxAuthorityId = dto.FgsSetupTaxAuthorityId;
        entity.EffectiveFromDate = dto.EffectiveFromDate;
        entity.EffectiveToDate = dto.EffectiveToDate;
        entity.TaxPercent = dto.TaxPercent;
        entity.IsExternalSystemRecord = dto.IsExternalSystemRecord;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTaxDetailDetailDto> PatchAsync(
        long id,
        FgsSetupTaxDetailPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax Detail '{id}' was not found.");

        if (dto.FgsSetupTaxId.HasValue)
        {
            entity.FgsSetupTaxId = dto.FgsSetupTaxId.Value;
        }
        if (dto.FgsSetupTaxAuthorityId.HasValue)
        {
            entity.FgsSetupTaxAuthorityId = dto.FgsSetupTaxAuthorityId.Value;
        }
        if (dto.EffectiveFromDate.HasValue)
        {
            entity.EffectiveFromDate = dto.EffectiveFromDate.Value;
        }
        if (dto.EffectiveToDate.HasValue)
        {
            entity.EffectiveToDate = dto.EffectiveToDate.Value;
        }
        if (dto.TaxPercent.HasValue)
        {
            entity.TaxPercent = dto.TaxPercent.Value;
        }
        if (dto.IsExternalSystemRecord.HasValue)
        {
            entity.IsExternalSystemRecord = dto.IsExternalSystemRecord.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTaxDetailDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax Detail '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupTaxDetail?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupTaxDetails.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A tax detail with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupTaxDetailDetailDto MapToDetail(FgsSetupTaxDetail entity) =>
        new(
            entity.Id,
            entity.TenantId,
            entity.CompanyId,
            entity.FgsSetupTaxId,
            entity.FgsSetupTaxAuthorityId,
            entity.EffectiveFromDate,
            entity.EffectiveToDate,
            entity.TaxPercent,
            entity.IsExternalSystemRecord,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy,
            entity.UpdatedOn,
            entity.UpdatedBy);
}
