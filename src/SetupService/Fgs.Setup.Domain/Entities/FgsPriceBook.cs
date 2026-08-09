using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Defines the master catalog of services offered by a company.
/// Each price book header represents a reusable service template used by estimates, work orders, invoices, scheduling, and pricing.
/// </summary>
public class FgsPriceBook : FgsTenantCompanySetupEntityBase<long>
{
    public string PriceBookCode { get; set; } = null!;

    public string PriceBookName { get; set; } = null!;

    public string? Description { get; set; }

    public long JobTypeId { get; set; }

    public string PricingModel { get; set; } = null!;

    public int EstimatedDurationMinutes { get; set; } = 60;

    public decimal? BasePrice { get; set; }

    public bool IsTaxable { get; set; } = true;

    public FgsJobType? JobType { get; set; }

    public ICollection<FgsPriceBookItem> Items { get; set; } = new List<FgsPriceBookItem>();
}
