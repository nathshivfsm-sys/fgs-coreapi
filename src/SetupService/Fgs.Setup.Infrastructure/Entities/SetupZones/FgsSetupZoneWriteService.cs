using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Entities.SetupZones;

public sealed class FgsSetupZoneWriteService : IFgsSetupZoneWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSetupZoneWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSetupZoneDetailDto> CreateAsync(
        FgsSetupZoneCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupZone
        {
            Code = NormalizeCode(dto.Code),
            Name = dto.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim()
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupZones.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupZoneDetailDto> UpdateAsync(
        long id,
        FgsSetupZoneUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Zone '{id}' was not found.");

        entity.Code = NormalizeCode(dto.Code);
        entity.Name = dto.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupZoneDetailDto> PatchAsync(
        long id,
        FgsSetupZonePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Zone '{id}' was not found.");

        if (dto.Code is not null)
        {
            entity.Code = NormalizeCode(dto.Code); ;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim(); ;
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

    public async Task<FgsSetupZoneDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Zone '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupZone?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupZones.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A zone with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupZoneDetailDto MapToDetail(FgsSetupZone entity) =>
        new(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.Description,
            entity.IsActive);
}
