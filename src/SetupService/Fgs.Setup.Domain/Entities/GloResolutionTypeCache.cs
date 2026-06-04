namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Local cache of <see cref="GloResolutionType"/> to avoid cross-schema FKs from setup to glo.
/// </summary>
public class GloResolutionTypeCache
{
    public int ResolutionTypeId { get; set; }

    public string ResolutionTypeCode { get; set; } = null!;

    public string ResolutionTypeName { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset? UpdatedOn { get; set; }
}
