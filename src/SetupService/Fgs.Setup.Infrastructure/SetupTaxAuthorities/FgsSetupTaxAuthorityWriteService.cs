using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.SetupTaxAuthorities;

public sealed class FgsSetupTaxAuthorityWriteService : IFgsSetupTaxAuthorityWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSetupTaxAuthorityWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSetupTaxAuthorityDetailDto> CreateAsync(
        FgsSetupTaxAuthorityCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupTaxAuthority
        {
            Code = NormalizeCode(dto.Code),
            Name = dto.Name.Trim(),
            RegionCode = string.IsNullOrWhiteSpace(dto.RegionCode) ? null : NormalizeCode(dto.RegionCode),
            IsExternalSystemRecord = dto.IsExternalSystemRecord,
            TaxPercent = dto.TaxPercent,
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim()
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupTaxAuthorities.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTaxAuthorityDetailDto> UpdateAsync(
        long id,
        FgsSetupTaxAuthorityUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax Authority '{id}' was not found.");

        entity.Code = NormalizeCode(dto.Code);
        entity.Name = dto.Name.Trim();
        entity.RegionCode = string.IsNullOrWhiteSpace(dto.RegionCode) ? null : NormalizeCode(dto.RegionCode);
        entity.IsExternalSystemRecord = dto.IsExternalSystemRecord;
        entity.TaxPercent = dto.TaxPercent;
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTaxAuthorityDetailDto> PatchAsync(
        long id,
        FgsSetupTaxAuthorityPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax Authority '{id}' was not found.");

        if (dto.Code is not null)
        {
            entity.Code = NormalizeCode(dto.Code);
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.RegionCode is not null)
        {
            entity.RegionCode = string.IsNullOrWhiteSpace(dto.RegionCode) ? null : NormalizeCode(dto.RegionCode);
        }

        if (dto.IsExternalSystemRecord.HasValue)
        {
            entity.IsExternalSystemRecord = dto.IsExternalSystemRecord.Value;
        }

        if (dto.TaxPercent.HasValue)
        {
            entity.TaxPercent = dto.TaxPercent.Value;
        }

        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTaxAuthorityDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax Authority '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupTaxAuthority?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupTaxAuthorities.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A tax authority with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupTaxAuthorityDetailDto MapToDetail(FgsSetupTaxAuthority entity) =>
        new(
            entity.Id,
            entity.TenantId,
            entity.CompanyId,
            entity.Code,
            entity.Name,
            entity.RegionCode,
            entity.IsExternalSystemRecord,
            entity.TaxPercent,
            entity.Description,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy,
            entity.UpdatedOn,
            entity.UpdatedBy);
}
