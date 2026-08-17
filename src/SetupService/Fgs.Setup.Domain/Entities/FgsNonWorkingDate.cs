namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Stores tenant/company specific calendar dates on which normal business operations are not scheduled.
/// </summary>
public class FgsNonWorkingDate : FgsTenantCompanySetupEntityBase<long>
{
    /// <summary>
    /// Calendar date on which the company does not operate under its normal working schedule.
    /// </summary>
    public DateOnly NonWorkingDate { get; set; }

    /// <summary>
    /// Name identifying the non-working date, such as New Year's Day, Thanksgiving, Company Holiday, or Emergency Closure.
    /// </summary>
    public string Name { get; set; } = null!;
}
