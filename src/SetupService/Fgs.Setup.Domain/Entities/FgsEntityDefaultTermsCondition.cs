namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Stores the default terms and conditions version assigned to each supported entity type for a tenant and company.
/// </summary>
public class FgsEntityDefaultTermsCondition : FgsTenantCompanySetupEntityBase<long>
{
    public string EntityType { get; set; } = null!;

    public long TermsConditionId { get; set; }

    public FgsTermsCondition? TermsCondition { get; set; }
}
