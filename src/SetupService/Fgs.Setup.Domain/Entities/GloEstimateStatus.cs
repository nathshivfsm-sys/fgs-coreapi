namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// System-defined estimate statuses used to seed tenant/company estimate statuses during provisioning.
/// </summary>
public class GloEstimateStatus
{
    public short Id { get; set; }

    public string StatusCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public short DisplayOrder { get; set; } = 1;

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
