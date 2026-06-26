using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.VehicleMaintenances;

public sealed class FgsVehicleMaintenanceWriteService : IFgsVehicleMaintenanceWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsVehicleMaintenanceWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsVehicleMaintenanceDetailDto> CreateAsync(
        FgsVehicleMaintenanceCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsVehicleMaintenance
        {
            VehicleId = dto.VehicleId, VehicleMaintenanceTypeId = dto.VehicleMaintenanceTypeId, ServiceDate = dto.ServiceDate, MileageAtService = dto.MileageAtService ?? 1, ServiceProvider = string.IsNullOrWhiteSpace(dto.ServiceProvider) ? null : dto.ServiceProvider.Trim(), InvoiceNumber = string.IsNullOrWhiteSpace(dto.InvoiceNumber) ? null : dto.InvoiceNumber.Trim(), Cost = dto.Cost, NextServiceDate = dto.NextServiceDate, NextServiceMileage = dto.NextServiceMileage ?? 1, IsCompleted = dto.IsCompleted, Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(), Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim()
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsVehicleMaintenances.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsVehicleMaintenanceDetailDto> UpdateAsync(
        long id,
        FgsVehicleMaintenanceUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vehicle Maintenance '{id}' was not found.");

        entity.VehicleId = dto.VehicleId;
        entity.VehicleMaintenanceTypeId = dto.VehicleMaintenanceTypeId;
        entity.ServiceDate = dto.ServiceDate;
        entity.MileageAtService = dto.MileageAtService ?? entity.MileageAtService;
        entity.ServiceProvider = string.IsNullOrWhiteSpace(dto.ServiceProvider) ? null : dto.ServiceProvider.Trim();
        entity.InvoiceNumber = string.IsNullOrWhiteSpace(dto.InvoiceNumber) ? null : dto.InvoiceNumber.Trim();
        entity.Cost = dto.Cost;
        entity.NextServiceDate = dto.NextServiceDate;
        entity.NextServiceMileage = dto.NextServiceMileage ?? entity.NextServiceMileage;
        entity.IsCompleted = dto.IsCompleted;
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsVehicleMaintenanceDetailDto> PatchAsync(
        long id,
        FgsVehicleMaintenancePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vehicle Maintenance '{id}' was not found.");

        if (dto.VehicleId.HasValue)
        {
            entity.VehicleId = dto.VehicleId.Value;
        }
        if (dto.VehicleMaintenanceTypeId.HasValue)
        {
            entity.VehicleMaintenanceTypeId = dto.VehicleMaintenanceTypeId.Value;
        }
        if (dto.ServiceDate.HasValue)
        {
            entity.ServiceDate = dto.ServiceDate.Value;
        }
        if (dto.MileageAtService.HasValue)
        {
            entity.MileageAtService = dto.MileageAtService.Value;
        }
        if (dto.ServiceProvider is not null)
        {
            entity.ServiceProvider = string.IsNullOrWhiteSpace(dto.ServiceProvider) ? null : dto.ServiceProvider.Trim();;
        }
        if (dto.InvoiceNumber is not null)
        {
            entity.InvoiceNumber = string.IsNullOrWhiteSpace(dto.InvoiceNumber) ? null : dto.InvoiceNumber.Trim();;
        }
        if (dto.Cost.HasValue)
        {
            entity.Cost = dto.Cost.Value;
        }
        if (dto.NextServiceDate.HasValue)
        {
            entity.NextServiceDate = dto.NextServiceDate.Value;
        }
        if (dto.NextServiceMileage.HasValue)
        {
            entity.NextServiceMileage = dto.NextServiceMileage.Value;
        }
        if (dto.IsCompleted.HasValue)
        {
            entity.IsCompleted = dto.IsCompleted.Value;
        }
        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();;
        }
        if (dto.Notes is not null)
        {
            entity.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsVehicleMaintenanceDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vehicle Maintenance '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsVehicleMaintenance?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsVehicleMaintenances.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A vehicle maintenance with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsVehicleMaintenanceDetailDto MapToDetail(FgsVehicleMaintenance entity) =>
        new(
            entity.Id,
            entity.VehicleId,
            entity.VehicleMaintenanceTypeId,
            entity.ServiceDate,
            entity.MileageAtService,
            entity.ServiceProvider,
            entity.InvoiceNumber,
            entity.Cost,
            entity.NextServiceDate,
            entity.NextServiceMileage,
            entity.IsCompleted,
            entity.Description,
            entity.Notes,
            entity.IsActive);
}
