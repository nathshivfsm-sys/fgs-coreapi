using Fgs.Setup.Application.Features.SetupTaxes.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.SetupTaxes;

internal sealed class FgsSetupTaxSummaryRow
{
    public long Id { get; set; }
    public string TaxCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsExternalSystemRecord { get; set; }
    public string? ExternalSystemId { get; set; }
    public string? SyncToken { get; set; }
    public bool ShowTaxDetail { get; set; }
    public string? Description { get; set; }
    public decimal TaxRate { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupTaxSummaryDto ToDto() =>
        new(
            Id,
            TaxCode,
            Name,
            ShowTaxDetail,
            Description,
            TaxRate,
            IsActive);
}

internal sealed class FgsSetupTaxDetailRow
{
    public long Id { get; set; }
    public string TaxCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsExternalSystemRecord { get; set; }
    public string? ExternalSystemId { get; set; }
    public string? SyncToken { get; set; }
    public bool ShowTaxDetail { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupTaxDetailDto ToDto(IReadOnlyList<FgsSetupTaxLineDetailDto> taxDetails) =>
        new(
            Id,
            TaxCode,
            Name,
            ShowTaxDetail,
            Description,
            taxDetails.Where(d => d.IsActive).Sum(d => d.TaxPercent),
            IsActive,
            taxDetails);
}

internal sealed class FgsSetupTaxLineDetailRow
{
    public long Id { get; set; }
    public long FgsSetupTaxAuthorityId { get; set; }
    public string TaxAuthorityCode { get; set; } = null!;
    public string TaxAuthorityName { get; set; } = null!;
    public decimal TaxPercent { get; set; }
    public DateOnly EffectiveFromDate { get; set; }
    public DateOnly? EffectiveToDate { get; set; }
    public bool IsExternalSystemRecord { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupTaxLineDetailDto ToDto() =>
        new(
            Id,
            FgsSetupTaxAuthorityId,
            TaxAuthorityCode,
            TaxAuthorityName,
            TaxPercent,
            EffectiveFromDate,
            EffectiveToDate,
            IsActive);
}

internal sealed class FgsSetupTaxLookupRow
{
    public long Id { get; set; }
    public string TaxCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal TaxRate { get; set; }

    public FgsSetupTaxLookupDto ToDto() => new(Id,
            TaxCode,
            Name,
            TaxRate);
}
