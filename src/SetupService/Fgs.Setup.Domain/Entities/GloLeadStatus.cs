namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of lead pipeline statuses used to seed tenant-specific records.
/// </summary>
public class GloLeadStatus : GloEntityBase
{
    public short Id { get; set; }

    public string StatusCode { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
