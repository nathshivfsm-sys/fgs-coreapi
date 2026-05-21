namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global grouping for credential / integration providers (e.g. payments, tax, communications).
/// </summary>
public class GloCredentialCategory : GloEntityBase
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
