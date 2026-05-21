namespace Fgs.User.Domain.Entities;

/// <summary>
/// Associates a GL break with one or more technician trades.
/// </summary>
public class FgsSetupGLBreakTechTrade : FgsTenantCompanySetupEntityBase<long>
{
    public long FgsSetupGLBreakId { get; set; }

    public long FgsSetupTechTradeId { get; set; }

    public FgsSetupGLBreak? GLBreak { get; set; }

    public FgsSetupTechTrade? TechTrade { get; set; }
}
