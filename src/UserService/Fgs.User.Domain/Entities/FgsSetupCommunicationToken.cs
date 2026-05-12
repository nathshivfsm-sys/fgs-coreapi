namespace Fgs.User.Domain.Entities;

public class FgsSetupCommunicationToken : FgsTenantCompanySetupEntityBase
{
    public string TokenCode { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? Description { get; set; }

    public string? SampleValue { get; set; }
}
