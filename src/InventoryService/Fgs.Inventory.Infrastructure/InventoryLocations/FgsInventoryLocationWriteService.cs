using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.InventoryLocations;

public sealed class FgsInventoryLocationWriteService : IFgsInventoryLocationWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;

    public FgsInventoryLocationWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsInventoryLocationDetailDto> CreateAsync(
        FgsInventoryLocationCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsInventoryLocation
        {
            InventoryLocationCode = NormalizeCode(dto.InventoryLocationCode),
            Name = dto.Name.Trim(),
            InventoryLocationType = dto.InventoryLocationType.Trim().ToUpperInvariant(),
            ParentInventoryLocationId = dto.ParentInventoryLocationId,
            Description = TrimOrNull(dto.Description),
            Address1 = TrimOrNull(dto.Address1),
            Address2 = TrimOrNull(dto.Address2),
            City = TrimOrNull(dto.City),
            StateProvince = TrimOrNull(dto.StateProvince),
            PostalCode = TrimOrNull(dto.PostalCode),
            Country = TrimOrNull(dto.Country),
            ContactName = TrimOrNull(dto.ContactName),
            PhoneNumber = TrimOrNull(dto.PhoneNumber),
            Email = TrimOrNull(dto.Email),
            IsDefault = dto.IsDefault
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsInventoryLocations.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsInventoryLocationDetailDto> UpdateAsync(
        long id,
        FgsInventoryLocationUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory location '{id}' was not found.");

        ApplyMutableFields(entity, dto.InventoryLocationCode, dto.Name, dto.InventoryLocationType, dto.ParentInventoryLocationId,
            dto.Description, dto.Address1, dto.Address2, dto.City, dto.StateProvince, dto.PostalCode, dto.Country,
            dto.ContactName, dto.PhoneNumber, dto.Email, dto.IsDefault);

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsInventoryLocationDetailDto> PatchAsync(
        long id,
        FgsInventoryLocationPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory location '{id}' was not found.");

        if (dto.InventoryLocationCode is not null)
        {
            entity.InventoryLocationCode = NormalizeCode(dto.InventoryLocationCode);
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.InventoryLocationType is not null)
        {
            entity.InventoryLocationType = dto.InventoryLocationType.Trim().ToUpperInvariant();
        }

        if (dto.ParentInventoryLocationId.HasValue)
        {
            entity.ParentInventoryLocationId = dto.ParentInventoryLocationId.Value;
        }

        if (dto.Description is not null)
        {
            entity.Description = TrimOrNull(dto.Description);
        }

        if (dto.Address1 is not null)
        {
            entity.Address1 = TrimOrNull(dto.Address1);
        }

        if (dto.Address2 is not null)
        {
            entity.Address2 = TrimOrNull(dto.Address2);
        }

        if (dto.City is not null)
        {
            entity.City = TrimOrNull(dto.City);
        }

        if (dto.StateProvince is not null)
        {
            entity.StateProvince = TrimOrNull(dto.StateProvince);
        }

        if (dto.PostalCode is not null)
        {
            entity.PostalCode = TrimOrNull(dto.PostalCode);
        }

        if (dto.Country is not null)
        {
            entity.Country = TrimOrNull(dto.Country);
        }

        if (dto.ContactName is not null)
        {
            entity.ContactName = TrimOrNull(dto.ContactName);
        }

        if (dto.PhoneNumber is not null)
        {
            entity.PhoneNumber = TrimOrNull(dto.PhoneNumber);
        }

        if (dto.Email is not null)
        {
            entity.Email = TrimOrNull(dto.Email);
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

    public async Task<FgsInventoryLocationDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory location '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsInventoryLocation?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsInventoryLocations.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("An inventory location with the same code already exists.", ex);
        }
    }

    private static void ApplyMutableFields(
        FgsInventoryLocation entity,
        string inventoryLocationCode,
        string name,
        string inventoryLocationType,
        long? parentInventoryLocationId,
        string? description,
        string? address1,
        string? address2,
        string? city,
        string? stateProvince,
        string? postalCode,
        string? country,
        string? contactName,
        string? phoneNumber,
        string? email,
        bool isDefault)
    {
        entity.InventoryLocationCode = NormalizeCode(inventoryLocationCode);
        entity.Name = name.Trim();
        entity.InventoryLocationType = inventoryLocationType.Trim().ToUpperInvariant();
        entity.ParentInventoryLocationId = parentInventoryLocationId;
        entity.Description = TrimOrNull(description);
        entity.Address1 = TrimOrNull(address1);
        entity.Address2 = TrimOrNull(address2);
        entity.City = TrimOrNull(city);
        entity.StateProvince = TrimOrNull(stateProvince);
        entity.PostalCode = TrimOrNull(postalCode);
        entity.Country = TrimOrNull(country);
        entity.ContactName = TrimOrNull(contactName);
        entity.PhoneNumber = TrimOrNull(phoneNumber);
        entity.Email = TrimOrNull(email);
        entity.IsDefault = isDefault;
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static FgsInventoryLocationDetailDto MapToDetail(FgsInventoryLocation entity) =>
        new(
            entity.Id,
            entity.InventoryLocationCode,
            entity.Name,
            entity.InventoryLocationType,
            entity.ParentInventoryLocationId,
            entity.Description,
            entity.Address1,
            entity.Address2,
            entity.City,
            entity.StateProvince,
            entity.PostalCode,
            entity.Country,
            entity.ContactName,
            entity.PhoneNumber,
            entity.Email,
            entity.IsDefault,
            entity.IsActive);
}
