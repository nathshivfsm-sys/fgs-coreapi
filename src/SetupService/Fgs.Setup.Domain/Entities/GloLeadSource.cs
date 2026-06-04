namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of customer / business acquisition sources.
/// </summary>
public class GloLeadSource : GloEntityBase
{
    public short Id { get; set; }

    public string SourceCode { get; set; } = null!;

    public string SourceName { get; set; } = null!;

    public string? Description { get; set; }
}
