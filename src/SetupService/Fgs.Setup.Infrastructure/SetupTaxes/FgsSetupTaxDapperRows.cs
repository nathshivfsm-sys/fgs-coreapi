using Fgs.Setup.Application.Features.SetupTaxes.Dtos;

namespace Fgs.Setup.Infrastructure.SetupTaxes;

internal sealed class FgsSetupTaxSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string TaxCode { get; set; }
    public string Name { get; set; }
    public bool IsExternalSystemRecord { get; set; }
    public string? ExternalSystemId { get; set; }
    public string? SyncToken { get; set; }
    public bool ShowTaxDetail { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSetupTaxSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            TaxCode,
            Name,
            IsExternalSystemRecord,
            ExternalSystemId,
            SyncToken,
            ShowTaxDetail,
            Description,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSetupTaxDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string TaxCode { get; set; }
    public string Name { get; set; }
    public bool IsExternalSystemRecord { get; set; }
    public string? ExternalSystemId { get; set; }
    public string? SyncToken { get; set; }
    public bool ShowTaxDetail { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSetupTaxDetailDto ToDto(IReadOnlyList<FgsSetupTaxAuthorityAssignmentDto> taxDetails) =>
        new(
            Id,
            TenantId,
            CompanyId,
            TaxCode,
            Name,
            IsExternalSystemRecord,
            ExternalSystemId,
            SyncToken,
            ShowTaxDetail,
            Description,
            taxDetails,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSetupTaxAuthorityAssignmentRow
{
    public long Id { get; set; }
    public long FgsSetupTaxAuthorityId { get; set; }
    public string TaxAuthorityCode { get; set; }
    public string TaxAuthorityName { get; set; }
    public decimal TaxPercent { get; set; }
    public DateOnly EffectiveFromDate { get; set; }
    public DateOnly? EffectiveToDate { get; set; }
    public bool IsExternalSystemRecord { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupTaxAuthorityAssignmentDto ToDto() =>
        new(
            Id,
            FgsSetupTaxAuthorityId,
            TaxAuthorityCode,
            TaxAuthorityName,
            TaxPercent,
            EffectiveFromDate,
            EffectiveToDate,
            IsExternalSystemRecord,
            IsActive);
}

internal sealed class FgsSetupTaxLookupRow
{
    public long Id { get; set; }
    public string TaxCode { get; set; }
    public string Name { get; set; }

    public FgsSetupTaxLookupDto ToDto() => new(
        Id,
        TaxCode,
        Name);
}
