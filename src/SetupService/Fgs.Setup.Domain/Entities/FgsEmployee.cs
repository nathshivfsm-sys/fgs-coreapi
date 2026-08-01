using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Employee master record for office and field personnel.
/// </summary>
public class FgsEmployee : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long? UserId { get; set; }

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

    public DateTime CreatedOn { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }
}
