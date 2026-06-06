namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global title-of-courtesy catalog (Mr., Mrs., Dr., etc.).
/// </summary>
public class GloTitleOfCourtesy
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
}
