namespace Fgs.User.Domain.Entities;

/// <summary>
/// Trade-to-GL-break mapping for financial segmentation and reporting.
/// </summary>
public class FgsSetupGLBreakTrade
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long GLBreakId { get; set; }

    public string TradeCode { get; set; } = null!;

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public FgsSetupGLBreak GLBreak { get; set; } = null!;
}
