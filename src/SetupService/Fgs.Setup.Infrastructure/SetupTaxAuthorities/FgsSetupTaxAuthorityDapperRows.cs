using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;

namespace Fgs.Setup.Infrastructure.SetupTaxAuthorities;

internal sealed class FgsSetupTaxAuthoritySummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? RegionCode { get; set; }
    public bool IsExternalSystemRecord { get; set; }
    public decimal TaxPercent { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSetupTaxAuthoritySummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Code,
            Name,
            RegionCode,
            IsExternalSystemRecord,
            TaxPercent,
            Description,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSetupTaxAuthorityDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? RegionCode { get; set; }
    public bool IsExternalSystemRecord { get; set; }
    public decimal TaxPercent { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSetupTaxAuthorityDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Code,
            Name,
            RegionCode,
            IsExternalSystemRecord,
            TaxPercent,
            Description,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSetupTaxAuthorityLookupRow
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal TaxPercent { get; set; }

    public FgsSetupTaxAuthorityLookupDto ToDto() => new(
        Id,
        Code,
        Name,
        TaxPercent);
}
