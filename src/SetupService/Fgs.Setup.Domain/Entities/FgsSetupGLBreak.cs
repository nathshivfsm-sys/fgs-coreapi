namespace Fgs.Setup.Domain.Entities;

public class FgsSetupGLBreak : FgsTenantCompanySetupEntityBase<long>
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? BreakLabel { get; set; }

    public short BreakLevel { get; set; } = 1;

    public long? LogoFileId { get; set; }

    public Guid? AddressId { get; set; }

    public ICollection<FgsSetupGLBreakTrade> Trades { get; set; } = [];
}
