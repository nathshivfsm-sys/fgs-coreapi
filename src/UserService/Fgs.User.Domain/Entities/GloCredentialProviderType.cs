namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global catalog of credential provider / integration product types (metadata only; secrets live off-DB).
/// </summary>
public class GloCredentialProviderType : GloEntityBase
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
