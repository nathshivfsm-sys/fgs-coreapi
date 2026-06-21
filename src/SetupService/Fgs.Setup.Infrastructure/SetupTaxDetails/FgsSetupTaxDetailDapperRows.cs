using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;

namespace Fgs.Setup.Infrastructure.SetupTaxDetails;

internal sealed class FgsSetupTaxDetailSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long FgsSetupTaxId { get; set; }
    public long FgsSetupTaxAuthorityId { get; set; }
    public DateOnly EffectiveFromDate { get; set; }
    public DateOnly? EffectiveToDate { get; set; }
    public decimal TaxPercent { get; set; }
    public bool IsExternalSystemRecord { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSetupTaxDetailSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            FgsSetupTaxId,
            FgsSetupTaxAuthorityId,
            EffectiveFromDate,
            EffectiveToDate,
            TaxPercent,
            IsExternalSystemRecord,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSetupTaxDetailDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long FgsSetupTaxId { get; set; }
    public long FgsSetupTaxAuthorityId { get; set; }
    public DateOnly EffectiveFromDate { get; set; }
    public DateOnly? EffectiveToDate { get; set; }
    public decimal TaxPercent { get; set; }
    public bool IsExternalSystemRecord { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSetupTaxDetailDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            FgsSetupTaxId,
            FgsSetupTaxAuthorityId,
            EffectiveFromDate,
            EffectiveToDate,
            TaxPercent,
            IsExternalSystemRecord,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSetupTaxDetailLookupRow
{
    public long Id { get; set; }
    public long FgsSetupTaxId { get; set; }
    public long FgsSetupTaxAuthorityId { get; set; }
    public DateOnly EffectiveFromDate { get; set; }
    public decimal TaxPercent { get; set; }

    public FgsSetupTaxDetailLookupDto ToDto() => new(Id,
            FgsSetupTaxId,
            FgsSetupTaxAuthorityId,
            EffectiveFromDate,
            TaxPercent);
}
