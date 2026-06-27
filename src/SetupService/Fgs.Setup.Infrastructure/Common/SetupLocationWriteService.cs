using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.Locations;
using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Common;

public sealed class SetupLocationWriteService : ISetupLocationWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public SetupLocationWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<Guid?> UpsertAsync(
        string masterEntityTypeCode,
        long entityNumber,
        Guid? existingLocationId,
        LocationWriteDto? address,
        CancellationToken cancellationToken = default)
    {
        if (address is null)
        {
            await SoftDeleteAsync(existingLocationId, cancellationToken);
            return null;
        }

        var masterEntityTypeId = await ResolveMasterEntityTypeIdAsync(masterEntityTypeCode, cancellationToken);

        if (existingLocationId is Guid locationId)
        {
            var entity = await _context.FgsLocations
                .FirstOrDefaultAsync(l => l.Id == locationId, cancellationToken);

            if (entity is not null)
            {
                ApplyWriteDto(entity, address);
                entity.MasterEntityTypeId = masterEntityTypeId;
                entity.EntityNumber = entityNumber;
                entity.IsActive = true;
                _auditHelper.StampForUpdate(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return entity.Id;
            }
        }

        var location = new FgsLocation { Id = Guid.NewGuid() };
        ApplyWriteDto(location, address);
        location.MasterEntityTypeId = masterEntityTypeId;
        location.EntityNumber = entityNumber;
        _auditHelper.StampForCreate(location);
        await _context.FgsLocations.AddAsync(location, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return location.Id;
    }

    public async Task SoftDeleteAsync(Guid? locationId, CancellationToken cancellationToken = default)
    {
        if (locationId is not Guid id)
        {
            return;
        }

        var entity = await _context.FgsLocations
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        if (entity is null || !entity.IsActive)
        {
            return;
        }

        entity.IsActive = false;
        _auditHelper.StampForUpdate(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<int> ResolveMasterEntityTypeIdAsync(
        string masterEntityTypeCode,
        CancellationToken cancellationToken)
    {
        var id = await _context.GloMasterEntityTypes
            .AsNoTracking()
            .Where(t => t.Code == masterEntityTypeCode)
            .Select(t => t.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (id == 0)
        {
            throw new InvalidOperationException(
                $"Master entity type '{masterEntityTypeCode}' was not found.");
        }

        return id;
    }

    private static void ApplyWriteDto(FgsLocation entity, LocationWriteDto dto)
    {
        entity.AddressLine1 = TrimOrNull(dto.AddressLine1);
        entity.AddressLine2 = TrimOrNull(dto.AddressLine2);
        entity.AddressLine3 = TrimOrNull(dto.AddressLine3);
        entity.AddressLine4 = TrimOrNull(dto.AddressLine4);
        entity.City = TrimOrNull(dto.City);
        entity.State = TrimOrNull(dto.State);
        entity.County = TrimOrNull(dto.County);
        entity.Country = TrimOrNull(dto.Country);
        entity.PostalCode = TrimOrNull(dto.PostalCode);
        entity.FormattedAddress = TrimOrNull(dto.FormattedAddress);
        entity.Latitude = dto.Latitude;
        entity.Longitude = dto.Longitude;
        entity.PlaceId = TrimOrNull(dto.PlaceId);
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
