namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// System-defined estimate flavors used to seed tenant/company estimate flavors during provisioning.
/// </summary>
public class GloEstimateFlavor
{
    public short Id { get; set; }

    public string FlavorCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string BackgroundColor { get; set; } = null!;

    public string TextColor { get; set; } = null!;

    public short DisplayOrder { get; set; } = 1;

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
