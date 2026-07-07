using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Entities.Vehicles;

public sealed class FgsVehicleWriteService : IFgsVehicleWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsVehicleWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsVehicleDetailDto> CreateAsync(
        FgsVehicleCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsVehicle
        {
            InventoryLocationId = dto.InventoryLocationId,
            OwnershipType = dto.OwnershipType.Trim(),
            OwnershipCompany = string.IsNullOrWhiteSpace(dto.OwnershipCompany) ? null : dto.OwnershipCompany.Trim(),
            Year = dto.Year ?? 1,
            Make = string.IsNullOrWhiteSpace(dto.Make) ? null : dto.Make.Trim(),
            Model = string.IsNullOrWhiteSpace(dto.Model) ? null : dto.Model.Trim(),
            Color = string.IsNullOrWhiteSpace(dto.Color) ? null : dto.Color.Trim(),
            VIN = dto.VIN.Trim(),
            LicensePlate = string.IsNullOrWhiteSpace(dto.LicensePlate) ? null : dto.LicensePlate.Trim(),
            LicensePlateState = string.IsNullOrWhiteSpace(dto.LicensePlateState) ? null : dto.LicensePlateState.Trim(),
            PurchaseDate = dto.PurchaseDate,
            PurchasePrice = dto.PurchasePrice,
            PurchasedFrom = string.IsNullOrWhiteSpace(dto.PurchasedFrom) ? null : dto.PurchasedFrom.Trim(),
            IsPurchasedNew = dto.IsPurchasedNew,
            Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim()
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsVehicles.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsVehicleDetailDto> UpdateAsync(
        long id,
        FgsVehicleUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vehicle '{id}' was not found.");

        entity.InventoryLocationId = dto.InventoryLocationId;
        entity.OwnershipType = dto.OwnershipType.Trim();
        entity.OwnershipCompany = string.IsNullOrWhiteSpace(dto.OwnershipCompany) ? null : dto.OwnershipCompany.Trim();
        entity.Year = dto.Year ?? entity.Year;
        entity.Make = string.IsNullOrWhiteSpace(dto.Make) ? null : dto.Make.Trim();
        entity.Model = string.IsNullOrWhiteSpace(dto.Model) ? null : dto.Model.Trim();
        entity.Color = string.IsNullOrWhiteSpace(dto.Color) ? null : dto.Color.Trim();
        entity.VIN = dto.VIN.Trim();
        entity.LicensePlate = string.IsNullOrWhiteSpace(dto.LicensePlate) ? null : dto.LicensePlate.Trim();
        entity.LicensePlateState = string.IsNullOrWhiteSpace(dto.LicensePlateState) ? null : dto.LicensePlateState.Trim();
        entity.PurchaseDate = dto.PurchaseDate;
        entity.PurchasePrice = dto.PurchasePrice;
        entity.PurchasedFrom = string.IsNullOrWhiteSpace(dto.PurchasedFrom) ? null : dto.PurchasedFrom.Trim();
        entity.IsPurchasedNew = dto.IsPurchasedNew;
        entity.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsVehicleDetailDto> PatchAsync(
        long id,
        FgsVehiclePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vehicle '{id}' was not found.");

        if (dto.InventoryLocationId.HasValue)
        {
            entity.InventoryLocationId = dto.InventoryLocationId.Value;
        }
        if (dto.OwnershipType is not null)
        {
            entity.OwnershipType = dto.OwnershipType.Trim(); ;
        }
        if (dto.OwnershipCompany is not null)
        {
            entity.OwnershipCompany = string.IsNullOrWhiteSpace(dto.OwnershipCompany) ? null : dto.OwnershipCompany.Trim(); ;
        }
        if (dto.Year.HasValue)
        {
            entity.Year = dto.Year.Value;
        }
        if (dto.Make is not null)
        {
            entity.Make = string.IsNullOrWhiteSpace(dto.Make) ? null : dto.Make.Trim(); ;
        }
        if (dto.Model is not null)
        {
            entity.Model = string.IsNullOrWhiteSpace(dto.Model) ? null : dto.Model.Trim(); ;
        }
        if (dto.Color is not null)
        {
            entity.Color = string.IsNullOrWhiteSpace(dto.Color) ? null : dto.Color.Trim(); ;
        }
        if (dto.VIN is not null)
        {
            entity.VIN = dto.VIN.Trim(); ;
        }
        if (dto.LicensePlate is not null)
        {
            entity.LicensePlate = string.IsNullOrWhiteSpace(dto.LicensePlate) ? null : dto.LicensePlate.Trim(); ;
        }
        if (dto.LicensePlateState is not null)
        {
            entity.LicensePlateState = string.IsNullOrWhiteSpace(dto.LicensePlateState) ? null : dto.LicensePlateState.Trim(); ;
        }
        if (dto.PurchaseDate.HasValue)
        {
            entity.PurchaseDate = dto.PurchaseDate.Value;
        }
        if (dto.PurchasePrice.HasValue)
        {
            entity.PurchasePrice = dto.PurchasePrice.Value;
        }
        if (dto.PurchasedFrom is not null)
        {
            entity.PurchasedFrom = string.IsNullOrWhiteSpace(dto.PurchasedFrom) ? null : dto.PurchasedFrom.Trim(); ;
        }
        if (dto.IsPurchasedNew.HasValue)
        {
            entity.IsPurchasedNew = dto.IsPurchasedNew.Value;
        }
        if (dto.Notes is not null)
        {
            entity.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim(); ;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsVehicleDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vehicle '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsVehicle?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsVehicles.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A vehicle with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsVehicleDetailDto MapToDetail(FgsVehicle entity) =>
        new(
            entity.Id,
            entity.InventoryLocationId,
            entity.OwnershipType,
            entity.OwnershipCompany,
            entity.Year,
            entity.Make,
            entity.Model,
            entity.Color,
            entity.VIN,
            entity.LicensePlate,
            entity.LicensePlateState,
            entity.PurchaseDate,
            entity.PurchasePrice,
            entity.PurchasedFrom,
            entity.IsPurchasedNew,
            entity.Notes,
            entity.IsActive);
}
