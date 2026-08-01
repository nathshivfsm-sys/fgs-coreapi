using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Abstractions.Locations;
using Fgs.Setup.Application.Features.Employees.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.Employees;

public sealed class FgsEmployeeWriteService : IFgsEmployeeWriteService
{
    private const string MasterEntityTypeCode = "EMPLOYEE";

    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;
    private readonly ISetupLocationWriteService _locationWriteService;

    public FgsEmployeeWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper,
        ISetupLocationWriteService locationWriteService)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
        _locationWriteService = locationWriteService;
    }

    public async Task<FgsEmployeeDetailDto> CreateAsync(
        FgsEmployeeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (overtimeRate, doubleTimeRate) = ResolveRates(dto.RegularRate, dto.OvertimeRate, dto.DoubleTimeRate);

        var entity = new FgsEmployee
        {
            UserId = dto.UserId,
            EmployeeNumber = dto.EmployeeNumber.Trim(),
            EmployeeTypeId = dto.EmployeeTypeId,
            DisplayName = dto.DisplayName.Trim(),
            LegalFirstName = dto.LegalFirstName.Trim(),
            LegalMiddleName = TrimOrNull(dto.LegalMiddleName),
            LegalLastName = dto.LegalLastName.Trim(),
            BirthDate = dto.BirthDate,
            HireDate = dto.HireDate,
            TerminationDate = dto.TerminationDate,
            StatusId = dto.StatusId,
            PersonalEmail = TrimOrNull(dto.PersonalEmail),
            OfficeEmail = TrimOrNull(dto.OfficeEmail),
            PersonalPhone = TrimOrNull(dto.PersonalPhone),
            OfficePhone = TrimOrNull(dto.OfficePhone),
            ProfilePhotoFileId = dto.ProfilePhotoFileId,
            RegularRate = dto.RegularRate,
            OvertimeRate = overtimeRate,
            DoubleTimeRate = doubleTimeRate,
            LaborBurdenTypeId = dto.LaborBurdenTypeId,
            LaborBurdenValue = dto.LaborBurdenValue,
            IsPurchaser = dto.IsPurchaser,
            Notes = TrimOrNull(dto.Notes)
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsEmployees.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        entity.AddressId = await _locationWriteService.UpsertAsync(
            MasterEntityTypeCode,
            entity.Id,
            null,
            dto.Address,
            cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsEmployeeDetailDto> UpdateAsync(
        long id,
        FgsEmployeeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee '{id}' was not found.");

        var (overtimeRate, doubleTimeRate) = ResolveRates(dto.RegularRate, dto.OvertimeRate, dto.DoubleTimeRate);

        entity.UserId = dto.UserId;
        entity.EmployeeNumber = dto.EmployeeNumber.Trim();
        entity.EmployeeTypeId = dto.EmployeeTypeId;
        entity.DisplayName = dto.DisplayName.Trim();
        entity.LegalFirstName = dto.LegalFirstName.Trim();
        entity.LegalMiddleName = TrimOrNull(dto.LegalMiddleName);
        entity.LegalLastName = dto.LegalLastName.Trim();
        entity.BirthDate = dto.BirthDate;
        entity.HireDate = dto.HireDate;
        entity.TerminationDate = dto.TerminationDate;
        entity.StatusId = dto.StatusId;
        entity.PersonalEmail = TrimOrNull(dto.PersonalEmail);
        entity.OfficeEmail = TrimOrNull(dto.OfficeEmail);
        entity.PersonalPhone = TrimOrNull(dto.PersonalPhone);
        entity.OfficePhone = TrimOrNull(dto.OfficePhone);
        entity.ProfilePhotoFileId = dto.ProfilePhotoFileId;
        entity.RegularRate = dto.RegularRate;
        entity.OvertimeRate = overtimeRate;
        entity.DoubleTimeRate = doubleTimeRate;
        entity.LaborBurdenTypeId = dto.LaborBurdenTypeId;
        entity.LaborBurdenValue = dto.LaborBurdenValue;
        entity.IsPurchaser = dto.IsPurchaser;
        entity.Notes = TrimOrNull(dto.Notes);

        entity.AddressId = await _locationWriteService.UpsertAsync(
            MasterEntityTypeCode,
            entity.Id,
            entity.AddressId,
            dto.Address,
            cancellationToken);

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsEmployeeDetailDto> PatchAsync(
        long id,
        FgsEmployeePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee '{id}' was not found.");

        if (dto.UserId.HasValue)
        {
            entity.UserId = dto.UserId.Value;
        }

        if (dto.EmployeeNumber is not null)
        {
            entity.EmployeeNumber = dto.EmployeeNumber.Trim();
        }

        if (dto.EmployeeTypeId.HasValue)
        {
            entity.EmployeeTypeId = dto.EmployeeTypeId.Value;
        }

        if (dto.DisplayName is not null)
        {
            entity.DisplayName = dto.DisplayName.Trim();
        }

        if (dto.LegalFirstName is not null)
        {
            entity.LegalFirstName = dto.LegalFirstName.Trim();
        }

        if (dto.LegalMiddleName is not null)
        {
            entity.LegalMiddleName = TrimOrNull(dto.LegalMiddleName);
        }

        if (dto.LegalLastName is not null)
        {
            entity.LegalLastName = dto.LegalLastName.Trim();
        }

        if (dto.BirthDate.HasValue)
        {
            entity.BirthDate = dto.BirthDate.Value;
        }

        if (dto.HireDate.HasValue)
        {
            entity.HireDate = dto.HireDate.Value;
        }

        if (dto.TerminationDate.HasValue)
        {
            entity.TerminationDate = dto.TerminationDate.Value;
        }

        if (dto.StatusId.HasValue)
        {
            entity.StatusId = dto.StatusId.Value;
        }

        if (dto.PersonalEmail is not null)
        {
            entity.PersonalEmail = TrimOrNull(dto.PersonalEmail);
        }

        if (dto.OfficeEmail is not null)
        {
            entity.OfficeEmail = TrimOrNull(dto.OfficeEmail);
        }

        if (dto.PersonalPhone is not null)
        {
            entity.PersonalPhone = TrimOrNull(dto.PersonalPhone);
        }

        if (dto.OfficePhone is not null)
        {
            entity.OfficePhone = TrimOrNull(dto.OfficePhone);
        }

        if (dto.ProfilePhotoFileId.HasValue)
        {
            entity.ProfilePhotoFileId = dto.ProfilePhotoFileId.Value;
        }

        if (dto.RegularRate.HasValue)
        {
            entity.RegularRate = dto.RegularRate.Value;
        }

        if (dto.OvertimeRate.HasValue)
        {
            entity.OvertimeRate = dto.OvertimeRate.Value;
        }

        if (dto.DoubleTimeRate.HasValue)
        {
            entity.DoubleTimeRate = dto.DoubleTimeRate.Value;
        }

        if (dto.RegularRate.HasValue && !dto.OvertimeRate.HasValue)
        {
            entity.OvertimeRate = dto.RegularRate.Value * 1.5m;
        }

        if (dto.RegularRate.HasValue && !dto.DoubleTimeRate.HasValue)
        {
            entity.DoubleTimeRate = dto.RegularRate.Value * 2m;
        }

        if (dto.LaborBurdenTypeId.HasValue)
        {
            entity.LaborBurdenTypeId = dto.LaborBurdenTypeId.Value;
        }

        if (dto.LaborBurdenValue.HasValue)
        {
            entity.LaborBurdenValue = dto.LaborBurdenValue.Value;
        }

        if (dto.IsPurchaser.HasValue)
        {
            entity.IsPurchaser = dto.IsPurchaser.Value;
        }

        if (dto.Notes is not null)
        {
            entity.Notes = TrimOrNull(dto.Notes);
        }

        if (dto.Address is not null)
        {
            entity.AddressId = await _locationWriteService.UpsertAsync(
                MasterEntityTypeCode,
                entity.Id,
                entity.AddressId,
                dto.Address,
                cancellationToken);
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsEmployeeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee '{id}' was not found.");

        if (entity.StatusId != EmployeeStatusIds.Inactive)
        {
            entity.StatusId = EmployeeStatusIds.Inactive;
            _auditHelper.StampForUpdate(entity);
            await _locationWriteService.SoftDeleteAsync(entity.AddressId, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    private async Task<FgsEmployee?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsEmployees.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task<FgsEmployeeDetailDto> MapToDetailAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await _context.FgsEmployees
            .AsNoTracking()
            .FirstAsync(e => e.Id == id, cancellationToken);

        FgsEmployeeAddressDetailDto? address = null;
        if (entity.AddressId is Guid addressId)
        {
            var location = await _context.FgsLocations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == addressId && l.IsActive, cancellationToken);

            if (location is not null)
            {
                address = MapLocation(location);
            }
        }

        return new FgsEmployeeDetailDto(
            entity.Id,
            entity.UserId,
            entity.EmployeeNumber,
            entity.EmployeeTypeId,
            entity.DisplayName,
            entity.LegalFirstName,
            entity.LegalMiddleName,
            entity.LegalLastName,
            entity.BirthDate,
            entity.HireDate,
            entity.TerminationDate,
            entity.StatusId,
            entity.PersonalEmail,
            entity.OfficeEmail,
            entity.PersonalPhone,
            entity.OfficePhone,
            address,
            entity.ProfilePhotoFileId,
            entity.RegularRate,
            entity.OvertimeRate,
            entity.DoubleTimeRate,
            entity.LaborBurdenTypeId,
            entity.LaborBurdenValue,
            entity.IsPurchaser,
            entity.Notes);
    }

    private static FgsEmployeeAddressDetailDto MapLocation(FgsLocation location) =>
        new(
            location.Id,
            location.AddressLine1,
            location.AddressLine2,
            location.City,
            location.State,
            location.Country,
            location.PostalCode);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("An employee with the same employee number or user link already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static (decimal? OvertimeRate, decimal? DoubleTimeRate) ResolveRates(
        decimal? regularRate,
        decimal? overtimeRate,
        decimal? doubleTimeRate)
    {
        if (!regularRate.HasValue)
        {
            return (overtimeRate, doubleTimeRate);
        }

        return (
            overtimeRate ?? regularRate.Value * 1.5m,
            doubleTimeRate ?? regularRate.Value * 2m);
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
