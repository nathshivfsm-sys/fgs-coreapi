namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global labor rate type master (Regular, Overtime, Double-Time, etc.).
/// </summary>
public class GloSetupLaborRateType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int SortOrder { get; set; }

    public bool IsSystem { get; set; } = true;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }
}
