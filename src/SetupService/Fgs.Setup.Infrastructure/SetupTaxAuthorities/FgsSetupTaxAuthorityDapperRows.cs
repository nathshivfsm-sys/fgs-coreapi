using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;

namespace Fgs.Setup.Infrastructure.SetupTaxAuthorities;

internal sealed class FgsSetupTaxAuthoritySummaryRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? RegionCode { get; set; }
    public bool IsExternalSystemRecord { get; set; }
    public decimal TaxPercent { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupTaxAuthoritySummaryDto ToDto() =>
        new(
            Id,
            Code,
            Name,
            RegionCode,
            IsExternalSystemRecord,
            TaxPercent,
            Description,
            IsActive);
}

internal sealed class FgsSetupTaxAuthorityDetailRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? RegionCode { get; set; }
    public bool IsExternalSystemRecord { get; set; }
    public decimal TaxPercent { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupTaxAuthorityDetailDto ToDto() =>
        new(
            Id,
            Code,
            Name,
            RegionCode,
            IsExternalSystemRecord,
            TaxPercent,
            Description,
            IsActive);
}

internal sealed class FgsSetupTaxAuthorityLookupRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal TaxPercent { get; set; }

    public FgsSetupTaxAuthorityLookupDto ToDto() => new(Id,
            Code,
            Name,
            TaxPercent);
}
