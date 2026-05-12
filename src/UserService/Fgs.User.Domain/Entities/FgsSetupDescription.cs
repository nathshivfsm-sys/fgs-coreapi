namespace Fgs.User.Domain.Entities;

public class FgsSetupDescription : FgsTenantCompanySetupEntityBase
{
    public string DescriptionTypeCode { get; set; } = null!;

    public string Body { get; set; } = null!;

    public long? FgsSetupTechTradeId { get; set; }

    public int SortOrder { get; set; }
}
