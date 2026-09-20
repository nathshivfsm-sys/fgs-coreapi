using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.SetupTaxes;

public sealed class FgsSetupTaxWriteService : IFgsSetupTaxWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;
    private readonly IFgsSetupTaxReadRepository _readRepository;

    public FgsSetupTaxWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper,
        IFgsSetupTaxReadRepository readRepository)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
        _readRepository = readRepository;
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

        SyncTaxDetails(entity, dto.TaxDetails ?? []);
        await SaveChangesAsync(cancellationToken);

        return await RequireDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsSetupTaxDetailDto> UpdateAsync(
        long id,
        FgsSetupTaxUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, includeDetails: true, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax '{id}' was not found.");

        entity.TaxCode = NormalizeCode(dto.TaxCode);
        entity.Name = dto.Name.Trim();
        entity.IsExternalSystemRecord = dto.IsExternalSystemRecord;
        entity.ExternalSystemId = string.IsNullOrWhiteSpace(dto.ExternalSystemId) ? null : dto.ExternalSystemId.Trim();
        entity.SyncToken = string.IsNullOrWhiteSpace(dto.SyncToken) ? null : dto.SyncToken.Trim();
        entity.ShowTaxDetail = dto.ShowTaxDetail;
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();

        SyncTaxDetails(entity, dto.TaxDetails ?? []);

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return await RequireDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsSetupTaxDetailDto> PatchAsync(
        long id,
        FgsSetupTaxPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, includeDetails: false, cancellationToken)
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

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return await RequireDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsSetupTaxDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, includeDetails: false, cancellationToken)
            ?? throw new KeyNotFoundException($"Tax '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return await RequireDetailAsync(entity.Id, cancellationToken);
    }

    private void SyncTaxDetails(
        FgsSetupTax tax,
        IReadOnlyList<FgsSetupTaxLineUpsertDto> details)
    {
        var existingById = tax.TaxDetails
            .Where(d => d.Id != 0)
            .ToDictionary(d => d.Id);
        var keep = new HashSet<FgsSetupTaxDetail>();

        foreach (var dto in details)
        {
            FgsSetupTaxDetail line;

            if (dto.Id is > 0 && existingById.TryGetValue(dto.Id.Value, out var found))
            {
                line = found;
                line.IsActive = true;
                _auditHelper.StampForUpdate(line);
            }
            else if (dto.Id is > 0)
            {
                throw new KeyNotFoundException($"Tax detail '{dto.Id.Value}' was not found on tax '{tax.Id}'.");
            }
            else
            {
                line = new FgsSetupTaxDetail { FgsSetupTaxId = tax.Id };
                _auditHelper.StampForCreate(line);
                tax.TaxDetails.Add(line);
            }

            line.FgsSetupTaxAuthorityId = dto.FgsSetupTaxAuthorityId;
            line.EffectiveFromDate = dto.EffectiveFromDate;
            line.EffectiveToDate = dto.EffectiveToDate;
            line.IsExternalSystemRecord = dto.IsExternalSystemRecord;
            keep.Add(line);
        }

        foreach (var omitted in tax.TaxDetails.Where(d => !keep.Contains(d) && d.IsActive))
        {
            omitted.IsActive = false;
            _auditHelper.StampForUpdate(omitted);
        }
    }

    private async Task<FgsSetupTax?> FindEntityAsync(
        long id,
        bool includeDetails,
        CancellationToken cancellationToken)
    {
        IQueryable<FgsSetupTax> query = _context.FgsSetupTaxes;
        if (includeDetails)
        {
            query = query.Include(e => e.TaxDetails);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    private async Task<FgsSetupTaxDetailDto> RequireDetailAsync(long id, CancellationToken cancellationToken) =>
        await _readRepository.GetByIdAsync(id, cancellationToken)
        ?? throw new KeyNotFoundException($"Tax '{id}' was not found.");

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
