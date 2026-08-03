using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPostalCodes;

internal sealed class FgsSetupPostalCodeSummaryRow
{
    public long Id { get; set; }
    public string PostalCode { get; set; } = null!;
    public string CountryCode { get; set; } = null!;
    public string StateProvinceCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public decimal TripChargeAmount { get; set; }
    public long? FgsSetupZoneId { get; set; }
    public long? FgsSetupTaxId { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupPostalCodeSummaryDto ToDto() =>
        new(
            Id,
            PostalCode,
            CountryCode,
            StateProvinceCode,
            City,
            TripChargeAmount,
            FgsSetupZoneId,
            FgsSetupTaxId,
            IsActive);
}

internal sealed class FgsSetupPostalCodeDetailRow
{
    public long Id { get; set; }
    public string PostalCode { get; set; } = null!;
    public string CountryCode { get; set; } = null!;
    public string StateProvinceCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public decimal TripChargeAmount { get; set; }
    public long? FgsSetupZoneId { get; set; }
    public long? FgsSetupTaxId { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupPostalCodeDetailDto ToDto() =>
        new(
            Id,
            PostalCode,
            CountryCode,
            StateProvinceCode,
            City,
            TripChargeAmount,
            FgsSetupZoneId,
            FgsSetupTaxId,
            IsActive);
}

internal sealed class FgsSetupPostalCodeLookupRow
{
    public long Id { get; set; }
    public string PostalCode { get; set; } = null!;

    public FgsSetupPostalCodeLookupDto ToDto() => new(Id, PostalCode);
}
