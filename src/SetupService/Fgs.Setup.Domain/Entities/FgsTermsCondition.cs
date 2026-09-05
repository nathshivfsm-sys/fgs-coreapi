namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Stores terms and conditions definitions and their versions for use across estimates, invoices,
/// work authorizations, signatures, and other business entities.
/// </summary>
public class FgsTermsCondition : FgsTenantCompanySetupEntityBase<long>
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int VersionNumber { get; set; }

    public string TermsText { get; set; } = null!;
}
