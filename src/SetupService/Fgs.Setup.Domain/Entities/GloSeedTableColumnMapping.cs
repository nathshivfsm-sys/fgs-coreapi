namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Column-level mapping for a <see cref="GloSeedTableMapping"/> seed operation.
/// </summary>
public class GloSeedTableColumnMapping : GloEntityBase
{
    public long Id { get; set; }

    public long SeedTableMappingId { get; set; }

    public string? SourceColumnName { get; set; }

    public string TargetColumnName { get; set; } = null!;

    public string? TransformationType { get; set; }

    public string? StaticValue { get; set; }

    public int ColumnOrder { get; set; }

    public bool IsRequired { get; set; }

    public string? Description { get; set; }

    public GloSeedTableMapping SeedTableMapping { get; set; } = null!;
}
