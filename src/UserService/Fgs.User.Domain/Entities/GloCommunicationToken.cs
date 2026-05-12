namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global merge-field tokens available in communication templates.
/// </summary>
public class GloCommunicationToken
{
    public int Id { get; set; }

    public string TokenCode { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string SourceTableName { get; set; } = null!;

    public string SourceColumnName { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }
}
