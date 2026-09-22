using Fgs.Contracts.Audit;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Outbox;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Abstractions.Locations;
using Fgs.Setup.Application.Features.Employees.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

using Fgs.MultiTenancy.Persistence;

namespace Fgs.Setup.Infrastructure.Persistence.Employees;

public sealed class FgsEmployeeWriteService : IFgsEmployeeWriteService
{
    private const string MasterEntityTypeCode = "EMPLOYEE";
    private const decimal DefaultDailyCapacityHours = 8.00m;
    private const string EmployeeUpdatedEventCode = "EMPLOYEE_UPDATED";
    private const string EmployeeCreatedEventCode = "EMPLOYEE_CREATED";
    private const string EmployeeStatusChangedEventCode = "EMPLOYEE_STATUS_CHANGED";
    private const string AuditEventSource = "API";
    // No EMPLOYEE record_type in audit DB yet (no schema migration). Use SYSTEM + EventCode.
    private const string AuditRecordType = "SYSTEM";
    private const string FieldChangeEntryType = "FIELD_CHANGE";
    private const string StatusFieldName = "Status";

    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;
    private readonly ISetupLocationWriteService _locationWriteService;
    private readonly IEmployeeAuditRecorder _employeeAuditRecorder;
    private readonly IOutboxWriter _outboxWriter;
    private readonly IFgsUserContext _userContext;

    public FgsEmployeeWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper,
        ISetupLocationWriteService locationWriteService,
        IEmployeeAuditRecorder employeeAuditRecorder,
        IOutboxWriter outboxWriter,
        IFgsUserContext userContext)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
        _locationWriteService = locationWriteService;
        _employeeAuditRecorder = employeeAuditRecorder;
        _outboxWriter = outboxWriter;
        _userContext = userContext;
    }

    public async Task<FgsEmployeeDetailDto> CreateAsync(
        FgsEmployeeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (overtimeRate, doubleTimeRate) = ResolveRates(dto.RegularRate, overtimeRate: null, doubleTimeRate: null);

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

        if (dto.EmployeeTypeId == EmployeeTypeIds.Technician || dto.TechnicianProfile is not null)
        {
            if (dto.TechnicianProfile is null)
            {
                throw new InvalidOperationException("Technician profile is required when EmployeeTypeId is Technician.");
            }

            await UpsertTechnicianProfileAsync(entity, dto.TechnicianProfile, cancellationToken);
        }

        await EnqueueEmployeeAuditAsync(
            entity,
            EmployeeCreatedEventCode,
            "Employee created.",
            details: null,
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

        var previousSnapshot = CaptureScalarSnapshot(entity);
        var previousStatusId = entity.StatusId;
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

        await SyncTechnicianProfileAsync(entity, dto.EmployeeTypeId, dto.TechnicianProfile, cancellationToken);
        await SyncAddressWithStatusAsync(entity, previousStatusId, cancellationToken);

        _auditHelper.StampForUpdate(entity);
        await EnqueueStatusAndFieldAuditsAsync(entity, previousStatusId, previousSnapshot, cancellationToken);
        await EnqueueEmployeeAccessChangedIfNeededAsync(entity, previousStatusId, cancellationToken);
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

        var previousSnapshot = CaptureScalarSnapshot(entity);
        var previousStatusId = entity.StatusId;

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

        ApplyStatusPatch(entity, dto);

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

        if (dto.EmployeeTypeId.HasValue || dto.TechnicianProfile is not null)
        {
            await SyncTechnicianProfileAsync(
                entity,
                entity.EmployeeTypeId,
                dto.TechnicianProfile,
                cancellationToken,
                patchMode: true);
        }

        await SyncAddressWithStatusAsync(entity, previousStatusId, cancellationToken);

        _auditHelper.StampForUpdate(entity);
        await EnqueueStatusAndFieldAuditsAsync(entity, previousStatusId, previousSnapshot, cancellationToken);
        await EnqueueEmployeeAccessChangedIfNeededAsync(entity, previousStatusId, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsEmployeeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee '{id}' was not found.");

        if (entity.StatusId != EmployeeStatusIds.Inactive)
        {
            var previousStatusId = entity.StatusId;
            var previousSnapshot = CaptureScalarSnapshot(entity);
            entity.StatusId = EmployeeStatusIds.Inactive;
            _auditHelper.StampForUpdate(entity);
            await _locationWriteService.SoftDeleteAsync(entity.AddressId, cancellationToken);
            await EnqueueStatusAndFieldAuditsAsync(entity, previousStatusId, previousSnapshot, cancellationToken);
            await EnqueueEmployeeAccessChangedIfNeededAsync(entity, previousStatusId, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    private async Task EnqueueStatusAndFieldAuditsAsync(
        FgsEmployee entity,
        short previousStatusId,
        IReadOnlyDictionary<string, string?> previousSnapshot,
        CancellationToken cancellationToken)
    {
        var currentSnapshot = CaptureScalarSnapshot(entity);
        var details = BuildFieldChangeDetails(previousSnapshot, currentSnapshot);

        if (previousStatusId != entity.StatusId)
        {
            var previousName = FormatStatusName(previousStatusId);
            var newName = FormatStatusName(entity.StatusId);
            await EnqueueEmployeeAuditAsync(
                entity,
                EmployeeStatusChangedEventCode,
                $"{previousName} → {newName}",
                [
                    new RecordAuditEventDetailRequest(
                        FieldChangeEntryType,
                        StatusFieldName,
                        previousName,
                        newName,
                        Sequence: 1)
                ],
                cancellationToken);

            details = details
                .Where(d => !string.Equals(d.ItemName, nameof(FgsEmployee.StatusId), StringComparison.Ordinal))
                .Select((d, index) => d with { Sequence = (short)(index + 1) })
                .ToList();
        }

        if (details.Count == 0)
        {
            return;
        }

        await EnqueueEmployeeAuditAsync(
            entity,
            EmployeeUpdatedEventCode,
            "Employee updated.",
            details,
            cancellationToken);
    }

    private Task EnqueueEmployeeAccessChangedIfNeededAsync(
        FgsEmployee entity,
        short previousStatusId,
        CancellationToken cancellationToken)
    {
        if (entity.UserId is not Guid userId)
        {
            return Task.CompletedTask;
        }

        var previousLoginActive = MapsToLoginActive(previousStatusId);
        var newLoginActive = MapsToLoginActive(entity.StatusId);
        if (previousLoginActive == newLoginActive)
        {
            return Task.CompletedTask;
        }

        return _outboxWriter.EnqueueEmployeeAccessChangedAsync(
            new EmployeeAccessChangedEvent(
                entity.TenantId,
                entity.CompanyId,
                entity.Id,
                userId,
                newLoginActive),
            Guid.NewGuid(),
            cancellationToken);
    }

    private static bool MapsToLoginActive(short statusId) => statusId == EmployeeStatusIds.Active;

    private static string FormatStatusName(short statusId) =>
        statusId switch
        {
            EmployeeStatusIds.Active => "Active",
            EmployeeStatusIds.Inactive => "Inactive",
            EmployeeStatusIds.LeaveOfAbsence => "LeaveOfAbsence",
            EmployeeStatusIds.Terminated => "Terminated",
            _ => statusId.ToString()
        };

    private Task EnqueueEmployeeAuditAsync(
        FgsEmployee entity,
        string eventCode,
        string summary,
        IReadOnlyList<RecordAuditEventDetailRequest>? details,
        CancellationToken cancellationToken) =>
        _employeeAuditRecorder.RecordAsync(
            new RecordAuditEventRequest(
                entity.TenantId,
                entity.CompanyId,
                eventCode,
                AuditEventSource,
                AuditRecordType,
                entity.Id,
                summary,
                OccurredOn: null,
                EntityNumber: entity.EmployeeNumber,
                UserName: _userContext.ResolveAuditActor(),
                Details: details),
            cancellationToken);

    private static IReadOnlyDictionary<string, string?> CaptureScalarSnapshot(FgsEmployee entity) =>
        new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            [nameof(FgsEmployee.UserId)] = FormatValue(entity.UserId),
            [nameof(FgsEmployee.EmployeeNumber)] = FormatValue(entity.EmployeeNumber),
            [nameof(FgsEmployee.EmployeeTypeId)] = FormatValue(entity.EmployeeTypeId),
            [nameof(FgsEmployee.DisplayName)] = FormatValue(entity.DisplayName),
            [nameof(FgsEmployee.LegalFirstName)] = FormatValue(entity.LegalFirstName),
            [nameof(FgsEmployee.LegalMiddleName)] = FormatValue(entity.LegalMiddleName),
            [nameof(FgsEmployee.LegalLastName)] = FormatValue(entity.LegalLastName),
            [nameof(FgsEmployee.BirthDate)] = FormatValue(entity.BirthDate),
            [nameof(FgsEmployee.HireDate)] = FormatValue(entity.HireDate),
            [nameof(FgsEmployee.TerminationDate)] = FormatValue(entity.TerminationDate),
            [nameof(FgsEmployee.StatusId)] = FormatValue(entity.StatusId),
            [nameof(FgsEmployee.PersonalEmail)] = FormatValue(entity.PersonalEmail),
            [nameof(FgsEmployee.OfficeEmail)] = FormatValue(entity.OfficeEmail),
            [nameof(FgsEmployee.PersonalPhone)] = FormatValue(entity.PersonalPhone),
            [nameof(FgsEmployee.OfficePhone)] = FormatValue(entity.OfficePhone),
            [nameof(FgsEmployee.AddressId)] = FormatValue(entity.AddressId),
            [nameof(FgsEmployee.ProfilePhotoFileId)] = FormatValue(entity.ProfilePhotoFileId),
            [nameof(FgsEmployee.RegularRate)] = FormatValue(entity.RegularRate),
            [nameof(FgsEmployee.OvertimeRate)] = FormatValue(entity.OvertimeRate),
            [nameof(FgsEmployee.DoubleTimeRate)] = FormatValue(entity.DoubleTimeRate),
            [nameof(FgsEmployee.LaborBurdenTypeId)] = FormatValue(entity.LaborBurdenTypeId),
            [nameof(FgsEmployee.LaborBurdenValue)] = FormatValue(entity.LaborBurdenValue),
            [nameof(FgsEmployee.IsPurchaser)] = FormatValue(entity.IsPurchaser),
            [nameof(FgsEmployee.Notes)] = FormatValue(entity.Notes)
        };

    private static IReadOnlyList<RecordAuditEventDetailRequest> BuildFieldChangeDetails(
        IReadOnlyDictionary<string, string?> previous,
        IReadOnlyDictionary<string, string?> current)
    {
        var details = new List<RecordAuditEventDetailRequest>();
        short sequence = 1;

        foreach (var (itemName, oldValue) in previous)
        {
            if (!current.TryGetValue(itemName, out var newValue))
            {
                continue;
            }

            if (string.Equals(oldValue, newValue, StringComparison.Ordinal))
            {
                continue;
            }

            details.Add(new RecordAuditEventDetailRequest(
                FieldChangeEntryType,
                itemName,
                oldValue,
                newValue,
                sequence++));
        }

        return details;
    }

    private static string? FormatValue(object? value) =>
        value switch
        {
            null => null,
            DateOnly date => date.ToString("yyyy-MM-dd"),
            decimal number => number.ToString("0.##"),
            bool flag => flag ? "true" : "false",
            _ => value.ToString()
        };

    private async Task SyncAddressWithStatusAsync(
        FgsEmployee employee,
        short previousStatusId,
        CancellationToken cancellationToken)
    {
        if (employee.StatusId == previousStatusId)
        {
            return;
        }

        if (employee.StatusId == EmployeeStatusIds.Inactive)
        {
            await _locationWriteService.SoftDeleteAsync(employee.AddressId, cancellationToken);
            return;
        }

        if (employee.StatusId == EmployeeStatusIds.Active
            && previousStatusId == EmployeeStatusIds.Inactive)
        {
            await _locationWriteService.ReactivateAsync(employee.AddressId, cancellationToken);
        }
    }

    private static void ApplyStatusPatch(FgsEmployee entity, FgsEmployeePatchDto dto)
    {
        if (dto.StatusId.HasValue)
        {
            entity.StatusId = dto.StatusId.Value;
            return;
        }

        if (dto.IsActive.HasValue)
        {
            entity.StatusId = dto.IsActive.Value
                ? EmployeeStatusIds.Active
                : EmployeeStatusIds.Inactive;
        }
    }

    private async Task SyncTechnicianProfileAsync(
        FgsEmployee employee,
        short employeeTypeId,
        FgsEmployeeTechnicianProfileWriteDto? profileDto,
        CancellationToken cancellationToken,
        bool patchMode = false)
    {
        if (employeeTypeId == EmployeeTypeIds.Office)
        {
            await RemoveTechnicianProfileAsync(employee.Id, cancellationToken);
            return;
        }

        if (profileDto is not null)
        {
            await UpsertTechnicianProfileAsync(employee, profileDto, cancellationToken);
            return;
        }

        if (patchMode)
        {
            // Patch to Technician without a profile payload keeps any existing profile.
            return;
        }

        if (employeeTypeId == EmployeeTypeIds.Technician)
        {
            throw new InvalidOperationException("Technician profile is required when EmployeeTypeId is Technician.");
        }
    }

    private async Task UpsertTechnicianProfileAsync(
        FgsEmployee employee,
        FgsEmployeeTechnicianProfileWriteDto dto,
        CancellationToken cancellationToken)
    {
        var existing = await _context.FgsEmployeeTechnicianProfiles
            .FirstOrDefaultAsync(p => p.EmployeeId == employee.Id, cancellationToken);

        if (existing is null)
        {
            var profile = new FgsEmployeeTechnicianProfile
            {
                EmployeeId = employee.Id
            };
            ApplyTechnicianProfileValues(profile, dto);
            _auditHelper.StampForCreate(profile);
            await _context.FgsEmployeeTechnicianProfiles.AddAsync(profile, cancellationToken);
            return;
        }

        ApplyTechnicianProfileValues(existing, dto);
        _auditHelper.StampForUpdate(existing);
    }

    private async Task RemoveTechnicianProfileAsync(long employeeId, CancellationToken cancellationToken)
    {
        var existing = await _context.FgsEmployeeTechnicianProfiles
            .FirstOrDefaultAsync(p => p.EmployeeId == employeeId, cancellationToken);

        if (existing is not null)
        {
            _context.FgsEmployeeTechnicianProfiles.Remove(existing);
        }
    }

    private static void ApplyTechnicianProfileValues(
        FgsEmployeeTechnicianProfile profile,
        FgsEmployeeTechnicianProfileWriteDto dto)
    {
        profile.TechCode = dto.TechCode.Trim();
        profile.TechName = TrimOrNull(dto.TechName);
        profile.CanBeScheduled = dto.CanBeScheduled;
        profile.DailyCapacityHours = dto.DailyCapacityHours ?? DefaultDailyCapacityHours;
        profile.DispatchZoneId = dto.DispatchZoneId;
        profile.StartLocationTypeId = dto.StartLocationTypeId;
        profile.StartTime = dto.StartTime;
        profile.TechTradeId = dto.TechTradeId;
        profile.TechSkillId = dto.TechSkillId;
        profile.TruckId = dto.TruckId;
        profile.CustomerFacingPhone = TrimOrNull(dto.CustomerFacingPhone);
        profile.Notes = TrimOrNull(dto.Notes);
    }

    private async Task<FgsEmployee?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsEmployees.FirstOrDefaultIncludingInactiveAsync(e => e.Id == id, cancellationToken);

    private async Task<FgsEmployeeDetailDto> MapToDetailAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await _context.FgsEmployees
            .AsNoTracking()
            .FirstAsync(e => e.Id == id, cancellationToken);

        FgsEmployeeAddressDetailDto? address = null;
        if (entity.AddressId is Guid addressId)
        {
            var location = await _context.FgsLocations
                .IgnoreQueryFilters()
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == addressId && l.IsActive, cancellationToken);

            if (location is not null)
            {
                address = MapLocation(location);
            }
        }

        var technicianProfile = await _context.FgsEmployeeTechnicianProfiles
            .AsNoTracking()
            .Where(p => p.EmployeeId == entity.Id)
            .Select(p => new FgsEmployeeTechnicianProfileDetailDto(
                p.Id,
                p.TechCode,
                p.TechName,
                p.CanBeScheduled,
                p.DailyCapacityHours,
                p.DispatchZoneId,
                p.StartLocationTypeId,
                p.StartTime,
                p.TechTradeId,
                p.TechSkillId,
                p.TruckId,
                p.CustomerFacingPhone,
                p.Notes))
            .FirstOrDefaultAsync(cancellationToken);

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
            entity.Notes,
            technicianProfile);
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
            throw new InvalidOperationException(
                "An employee with the same employee number, user link, or technician code already exists.",
                ex);
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
