namespace Fgs.User.Domain.Entities;

/// <summary>
/// Global resolution outcome types for field service workflows (completed, cancelled, etc.).
/// </summary>
public class GloResolutionType : FgsEntityBase
{
    public int Id { get; set; }

    public string ResolutionTypeCode { get; set; } = null!;

    public string ResolutionTypeName { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}
