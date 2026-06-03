namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global merge-field tokens available in communication templates.
/// </summary>
public class GloCommunicationToken : FgsEntityBase
{
    public int Id { get; set; }

    public string TokenCode { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string SourceDatabaseName { get; set; } = null!;

    public string SourceSchemaName { get; set; } = null!;

    public string SourceTableName { get; set; } = null!;

    public string SourceColumnName { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}
