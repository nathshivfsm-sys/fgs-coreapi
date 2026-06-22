using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
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

        await SyncTaxDetailsAsync(entity, dto.TaxDetails ?? [], cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsSetupTaxDetailDto> UpdateAsync(
        long id,
        FgsSetupTaxUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityWithDetailsAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax '{id}' was not found.");

        entity.TaxCode = NormalizeCode(dto.TaxCode);
        entity.Name = dto.Name.Trim();
        entity.IsExternalSystemRecord = dto.IsExternalSystemRecord;
        entity.ExternalSystemId = string.IsNullOrWhiteSpace(dto.ExternalSystemId) ? null : dto.ExternalSystemId.Trim();
        entity.SyncToken = string.IsNullOrWhiteSpace(dto.SyncToken) ? null : dto.SyncToken.Trim();
        entity.ShowTaxDetail = dto.ShowTaxDetail;
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();

        _auditHelper.StampForUpdate(entity);
        await SyncTaxDetailsAsync(entity, dto.TaxDetails ?? [], cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsSetupTaxDetailDto> PatchAsync(
        long id,
        FgsSetupTaxPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityWithDetailsAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax '{id}' was not found.");

        if (dto.TaxCode is not null)
        {
            entity.TaxCode = NormalizeCode(dto.TaxCode);
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.IsExternalSystemRecord.HasValue)
        {
            entity.IsExternalSystemRecord = dto.IsExternalSystemRecord.Value;
        }

        if (dto.ExternalSystemId is not null)
        {
            entity.ExternalSystemId = string.IsNullOrWhiteSpace(dto.ExternalSystemId) ? null : dto.ExternalSystemId.Trim();
        }

        if (dto.SyncToken is not null)
        {
            entity.SyncToken = string.IsNullOrWhiteSpace(dto.SyncToken) ? null : dto.SyncToken.Trim();
        }

        if (dto.ShowTaxDetail.HasValue)
        {
            entity.ShowTaxDetail = dto.ShowTaxDetail.Value;
        }

        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        if (dto.TaxDetails is not null)
        {
            await SyncTaxDetailsAsync(entity, dto.TaxDetails, cancellationToken);
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsSetupTaxDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityWithDetailsAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            foreach (var detail in entity.TaxDetails.Where(d => d.IsActive))
            {
                detail.IsActive = false;
                _auditHelper.StampForUpdate(detail);
            }

            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    private async Task SyncTaxDetailsAsync(
        FgsSetupTax entity,
        IReadOnlyList<FgsSetupTaxAuthorityAssignmentWriteDto> desired,
        CancellationToken cancellationToken)
    {
        var authorityIds = desired.Select(d => d.FgsSetupTaxAuthorityId).Distinct().ToList();
        if (authorityIds.Count > 0)
        {
            var existingCount = await _context.FgsSetupTaxAuthorities
                .CountAsync(
                    a => a.TenantId == entity.TenantId
                         && a.CompanyId == entity.CompanyId
                         && authorityIds.Contains(a.Id),
                    cancellationToken);

            if (existingCount != authorityIds.Count)
            {
                throw new InvalidOperationException("One or more tax authorities were not found.");
            }
        }

        var desiredKeys = desired
            .Select(d => (d.FgsSetupTaxAuthorityId, d.EffectiveFromDate))
            .ToHashSet();

        foreach (var detail in entity.TaxDetails.Where(d => d.IsActive).ToList())
        {
            var key = (detail.FgsSetupTaxAuthorityId, detail.EffectiveFromDate);
            if (!desiredKeys.Contains(key))
            {
                detail.IsActive = false;
                _auditHelper.StampForUpdate(detail);
            }
        }

        foreach (var assignment in desired)
        {
            var existing = entity.TaxDetails.FirstOrDefault(d =>
                d.FgsSetupTaxAuthorityId == assignment.FgsSetupTaxAuthorityId
                && d.EffectiveFromDate == assignment.EffectiveFromDate);

            if (existing is null)
            {
                var detail = new FgsSetupTaxDetail
                {
                    FgsSetupTaxId = entity.Id,
                    FgsSetupTaxAuthorityId = assignment.FgsSetupTaxAuthorityId,
                    EffectiveFromDate = assignment.EffectiveFromDate,
                    EffectiveToDate = assignment.EffectiveToDate,
                    IsExternalSystemRecord = assignment.IsExternalSystemRecord
                };

                _auditHelper.StampForCreate(detail);
                entity.TaxDetails.Add(detail);
                continue;
            }

            existing.EffectiveToDate = assignment.EffectiveToDate;
            existing.IsExternalSystemRecord = assignment.IsExternalSystemRecord;
            existing.IsActive = true;
            _auditHelper.StampForUpdate(existing);
        }
    }

    private async Task<FgsSetupTax?> FindEntityWithDetailsAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupTaxes
            .Include(e => e.TaxDetails)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task<FgsSetupTaxDetailDto> MapToDetailAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await _context.FgsSetupTaxes
            .AsNoTracking()
            .Include(e => e.TaxDetails)
            .FirstAsync(e => e.Id == id, cancellationToken);

        var activeDetails = entity.TaxDetails.Where(d => d.IsActive).ToList();
        var authorityIds = activeDetails.Select(d => d.FgsSetupTaxAuthorityId).Distinct().ToList();
        var authorities = authorityIds.Count == 0
            ? []
            : await _context.FgsSetupTaxAuthorities
                .AsNoTracking()
                .Where(a => authorityIds.Contains(a.Id))
                .ToDictionaryAsync(a => a.Id, cancellationToken);

        var taxDetails = activeDetails
            .OrderBy(d => d.EffectiveFromDate)
            .ThenBy(d => d.FgsSetupTaxAuthorityId)
            .Select(d =>
            {
                authorities.TryGetValue(d.FgsSetupTaxAuthorityId, out var authority);
                return new FgsSetupTaxAuthorityAssignmentDto(
                    d.Id,
                    d.FgsSetupTaxAuthorityId,
                    authority?.Code ?? string.Empty,
                    authority?.Name ?? string.Empty,
                    authority?.TaxPercent ?? 0m,
                    d.EffectiveFromDate,
                    d.EffectiveToDate,
                    d.IsExternalSystemRecord,
                    d.IsActive);
            })
            .ToList();

        return new FgsSetupTaxDetailDto(
            entity.Id,
            entity.TenantId,
            entity.CompanyId,
            entity.TaxCode,
            entity.Name,
            entity.IsExternalSystemRecord,
            entity.ExternalSystemId,
            entity.SyncToken,
            entity.ShowTaxDetail,
            entity.Description,
            taxDetails,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy,
            entity.UpdatedOn,
            entity.UpdatedBy);
    }

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
}
