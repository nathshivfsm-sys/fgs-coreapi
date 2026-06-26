using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Warehouses;

public sealed class FgsWarehouseWriteService : IFgsWarehouseWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsWarehouseWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsWarehouseDetailDto> CreateAsync(
        FgsWarehouseCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsWarehouse
        {
            WarehouseCode = NormalizeCode(dto.WarehouseCode), Name = dto.Name.Trim(), WarehouseType = dto.WarehouseType.Trim(), AddressId = dto.AddressId, Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(), IsDefault = dto.IsDefault
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsWarehouses.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsWarehouseDetailDto> UpdateAsync(
        long id,
        FgsWarehouseUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Warehouse '{id}' was not found.");

        entity.WarehouseCode = NormalizeCode(dto.WarehouseCode);
        entity.Name = dto.Name.Trim();
        entity.WarehouseType = dto.WarehouseType.Trim();
        entity.AddressId = dto.AddressId;
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.IsDefault = dto.IsDefault;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsWarehouseDetailDto> PatchAsync(
        long id,
        FgsWarehousePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Warehouse '{id}' was not found.");

        if (dto.WarehouseCode is not null)
        {
            entity.WarehouseCode = NormalizeCode(dto.WarehouseCode);;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();;
        }
        if (dto.WarehouseType is not null)
        {
            entity.WarehouseType = dto.WarehouseType.Trim();;
        }
        if (dto.AddressId.HasValue)
        {
            entity.AddressId = dto.AddressId.Value;
        }
        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();;
        }
        if (dto.IsDefault.HasValue)
        {
            entity.IsDefault = dto.IsDefault.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsWarehouseDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Warehouse '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsWarehouse?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsWarehouses.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A warehouse with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsWarehouseDetailDto MapToDetail(FgsWarehouse entity) =>
        new(
            entity.Id,
            entity.WarehouseCode,
            entity.Name,
            entity.WarehouseType,
            entity.AddressId,
            entity.Description,
            entity.IsDefault,
            entity.IsActive);
}
