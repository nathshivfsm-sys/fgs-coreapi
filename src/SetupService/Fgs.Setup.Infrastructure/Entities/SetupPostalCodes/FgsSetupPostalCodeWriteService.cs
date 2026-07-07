using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Entities.SetupPostalCodes;

public sealed class FgsSetupPostalCodeWriteService : IFgsSetupPostalCodeWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSetupPostalCodeWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSetupPostalCodeDetailDto> CreateAsync(
        FgsSetupPostalCodeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupPostalCode
        {
            PostalCode = dto.PostalCode.Trim(),
            FgsSetupZoneId = dto.FgsSetupZoneId,
            FgsSetupTaxId = dto.FgsSetupTaxId
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupPostalCodes.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPostalCodeDetailDto> UpdateAsync(
        long id,
        FgsSetupPostalCodeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Postal Code '{id}' was not found.");

        entity.PostalCode = dto.PostalCode.Trim();
        entity.FgsSetupZoneId = dto.FgsSetupZoneId;
        entity.FgsSetupTaxId = dto.FgsSetupTaxId;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPostalCodeDetailDto> PatchAsync(
        long id,
        FgsSetupPostalCodePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Postal Code '{id}' was not found.");

        if (dto.PostalCode is not null)
        {
            entity.PostalCode = dto.PostalCode.Trim(); ;
        }
        if (dto.FgsSetupZoneId.HasValue)
        {
            entity.FgsSetupZoneId = dto.FgsSetupZoneId.Value;
        }
        if (dto.FgsSetupTaxId.HasValue)
        {
            entity.FgsSetupTaxId = dto.FgsSetupTaxId.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPostalCodeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Postal Code '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupPostalCode?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupPostalCodes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A postal code with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupPostalCodeDetailDto MapToDetail(FgsSetupPostalCode entity) =>
        new(
            entity.Id,
            entity.PostalCode,
            entity.FgsSetupZoneId,
            entity.FgsSetupTaxId,
            entity.IsActive);
}
