namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global system tag catalog used as defaults during tenant onboarding.
/// </summary>
public class GloTag
{
    public int Id { get; set; }

    public string TagCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public string? Description { get; set; }

    public string? BackgroundColor { get; set; }

    public string? TextColor { get; set; }

    public long? IconFileId { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsSystemGenerated { get; set; } = true;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

}
