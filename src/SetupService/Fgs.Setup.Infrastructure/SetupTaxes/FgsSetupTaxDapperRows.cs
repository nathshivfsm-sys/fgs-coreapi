using Fgs.Setup.Application.Features.SetupTaxes.Dtos;

namespace Fgs.Setup.Infrastructure.SetupTaxes;

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
    public bool IsActive { get; set; }

    public FgsSetupTaxSummaryDto ToDto() =>
        new(
            Id,
            TaxCode,
            Name,
            IsExternalSystemRecord,
            ExternalSystemId,
            SyncToken,
            ShowTaxDetail,
            Description,
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

    public FgsSetupTaxDetailDto ToDto() =>
        new(
            Id,
            TaxCode,
            Name,
            IsExternalSystemRecord,
            ExternalSystemId,
            SyncToken,
            ShowTaxDetail,
            Description,
            IsActive);
}

internal sealed class FgsSetupTaxLookupRow
{
    public long Id { get; set; }
    public string TaxCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsSetupTaxLookupDto ToDto() => new(Id,
            TaxCode,
            Name);
}
