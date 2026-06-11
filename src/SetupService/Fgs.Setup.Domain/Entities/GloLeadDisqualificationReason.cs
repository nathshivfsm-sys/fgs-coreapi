namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of lead disqualification reasons used to seed tenant-specific records.
/// </summary>
public class GloLeadDisqualificationReason : GloEntityBase
{
    public short Id { get; set; }

    public string ReasonCode { get; set; } = null!;

    public string ReasonName { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
