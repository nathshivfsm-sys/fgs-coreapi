using Fgs.Setup.Application.Features.Employees.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.Employees;

internal sealed class FgsEmployeeSummaryRow
{
    public long Id { get; set; }
    public Guid? UserId { get; set; }
    public string EmployeeNumber { get; set; } = null!;
    public short EmployeeTypeId { get; set; }
    public string DisplayName { get; set; } = null!;
    public string LegalFirstName { get; set; } = null!;
    public string? LegalMiddleName { get; set; }
    public string LegalLastName { get; set; } = null!;
    public DateOnly? BirthDate { get; set; }
    public DateOnly? HireDate { get; set; }
    public DateOnly? TerminationDate { get; set; }
    public short StatusId { get; set; }
    public string? PersonalEmail { get; set; }
    public string? OfficeEmail { get; set; }
    public string? PersonalPhone { get; set; }
    public string? OfficePhone { get; set; }
    public long? ProfilePhotoFileId { get; set; }
    public decimal? RegularRate { get; set; }
    public decimal? OvertimeRate { get; set; }
    public decimal? DoubleTimeRate { get; set; }
    public short? LaborBurdenTypeId { get; set; }
    public decimal? LaborBurdenValue { get; set; }
    public bool IsPurchaser { get; set; }
    public string? Notes { get; set; }
    public bool HasTechnicianProfile { get; set; }

    public FgsEmployeeSummaryDto ToDto() =>
        new(
            Id,
            UserId,
            EmployeeNumber,
            EmployeeTypeId,
            DisplayName,
            LegalFirstName,
            LegalMiddleName,
            LegalLastName,
            BirthDate,
            HireDate,
            TerminationDate,
            StatusId,
            PersonalEmail,
            OfficeEmail,
            PersonalPhone,
            OfficePhone,
            ProfilePhotoFileId,
            RegularRate,
            OvertimeRate,
            DoubleTimeRate,
            LaborBurdenTypeId,
            LaborBurdenValue,
            IsPurchaser,
            Notes,
            HasTechnicianProfile);
}

internal sealed class FgsEmployeeListSummaryRow
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int InactiveEmployees { get; set; }

    public FgsEmployeeListSummaryDto ToDto() =>
        new(TotalEmployees, ActiveEmployees, InactiveEmployees);
}

internal sealed class FgsEmployeeDetailRow
{
    public long Id { get; set; }
    public Guid? UserId { get; set; }
    public string EmployeeNumber { get; set; } = null!;
    public short EmployeeTypeId { get; set; }
    public string DisplayName { get; set; } = null!;
    public string LegalFirstName { get; set; } = null!;
    public string? LegalMiddleName { get; set; }
    public string LegalLastName { get; set; } = null!;
    public DateOnly? BirthDate { get; set; }
    public DateOnly? HireDate { get; set; }
    public DateOnly? TerminationDate { get; set; }
    public short StatusId { get; set; }
    public string? PersonalEmail { get; set; }
    public string? OfficeEmail { get; set; }
    public string? PersonalPhone { get; set; }
    public string? OfficePhone { get; set; }
    public Guid? AddressId { get; set; }
    public long? ProfilePhotoFileId { get; set; }
    public decimal? RegularRate { get; set; }
    public decimal? OvertimeRate { get; set; }
    public decimal? DoubleTimeRate { get; set; }
    public short? LaborBurdenTypeId { get; set; }
    public decimal? LaborBurdenValue { get; set; }
    public bool IsPurchaser { get; set; }
    public string? Notes { get; set; }

    public Guid? LocationId { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }

    public long? TechnicianProfileId { get; set; }
    public string? TechCode { get; set; }
    public string? TechName { get; set; }
    public bool? CanBeScheduled { get; set; }
    public decimal? DailyCapacityHours { get; set; }
    public long? DispatchZoneId { get; set; }
    public short? StartLocationTypeId { get; set; }
    public TimeOnly? StartTime { get; set; }
    public int? TechTradeId { get; set; }
    public int? TechSkillId { get; set; }
    public long? TruckId { get; set; }
    public string? TechnicianCustomerFacingPhone { get; set; }
    public string? TechnicianNotes { get; set; }

    public FgsEmployeeDetailDto ToDto() =>
        new(
            Id,
            UserId,
            EmployeeNumber,
            EmployeeTypeId,
            DisplayName,
            LegalFirstName,
            LegalMiddleName,
            LegalLastName,
            BirthDate,
            HireDate,
            TerminationDate,
            StatusId,
            PersonalEmail,
            OfficeEmail,
            PersonalPhone,
            OfficePhone,
            ToAddressDto(),
            ProfilePhotoFileId,
            RegularRate,
            OvertimeRate,
            DoubleTimeRate,
            LaborBurdenTypeId,
            LaborBurdenValue,
            IsPurchaser,
            Notes,
            ToTechnicianProfileDto());

    private FgsEmployeeAddressDetailDto? ToAddressDto()
    {
        if (LocationId is not Guid locationId)
        {
            return null;
        }

        return new FgsEmployeeAddressDetailDto(
            locationId,
            AddressLine1,
            AddressLine2,
            City,
            State,
            Country,
            PostalCode);
    }

    private FgsEmployeeTechnicianProfileDetailDto? ToTechnicianProfileDto()
    {
        if (TechnicianProfileId is not long profileId || string.IsNullOrWhiteSpace(TechCode))
        {
            return null;
        }

        return new FgsEmployeeTechnicianProfileDetailDto(
            profileId,
            TechCode,
            TechName,
            CanBeScheduled ?? true,
            DailyCapacityHours ?? 8.00m,
            DispatchZoneId,
            StartLocationTypeId ?? 0,
            StartTime,
            TechTradeId,
            TechSkillId,
            TruckId,
            TechnicianCustomerFacingPhone,
            TechnicianNotes);
    }
}

internal sealed class FgsEmployeeLookupRow
{
    public long Id { get; set; }
    public string EmployeeNumber { get; set; } = null!;
    public string DisplayName { get; set; } = null!;

    public FgsEmployeeLookupDto ToDto() => new(Id, EmployeeNumber, DisplayName);
}
