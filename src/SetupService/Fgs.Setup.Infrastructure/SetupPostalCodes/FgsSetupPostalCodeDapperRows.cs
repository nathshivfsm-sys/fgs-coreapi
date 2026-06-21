using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;

namespace Fgs.Setup.Infrastructure.SetupPostalCodes;

internal sealed class FgsSetupPostalCodeSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string PostalCode { get; set; }
    public long? FgsSetupZoneId { get; set; }
    public long? FgsSetupTaxId { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSetupPostalCodeSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            PostalCode,
            FgsSetupZoneId,
            FgsSetupTaxId,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSetupPostalCodeDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string PostalCode { get; set; }
    public long? FgsSetupZoneId { get; set; }
    public long? FgsSetupTaxId { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSetupPostalCodeDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            PostalCode,
            FgsSetupZoneId,
            FgsSetupTaxId,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSetupPostalCodeLookupRow
{
    public long Id { get; set; }
    public string PostalCode { get; set; }

    public FgsSetupPostalCodeLookupDto ToDto() => new(Id,
            PostalCode);
}
