namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Defines a source-to-target table mapping used during tenant or global seed operations.
/// </summary>
public class GloSeedTableMapping : GloEntityBase
{
    public long Id { get; set; }

    public string SeedCode { get; set; } = null!;

    public string? SourceDatabaseName { get; set; }

    public string SourceSchemaName { get; set; } = "public";

    public string SourceTableName { get; set; } = null!;

    public string? TargetDatabaseName { get; set; }

    public string TargetSchemaName { get; set; } = "public";

    public string TargetTableName { get; set; } = null!;

    public int SeedOrder { get; set; }

    public string? Description { get; set; }

    public ICollection<GloSeedTableColumnMapping> ColumnMappings { get; set; } = [];
}
