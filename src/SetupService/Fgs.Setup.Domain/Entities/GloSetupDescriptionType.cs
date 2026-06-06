namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of setup description categories (reason for call, recommendations, etc.).
/// </summary>
public class GloSetupDescriptionType
{
    public short Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }
}
