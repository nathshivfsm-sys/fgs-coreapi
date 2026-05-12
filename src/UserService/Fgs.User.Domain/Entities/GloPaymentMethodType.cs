namespace Fgs.User.Domain.Entities;

public class GloPaymentMethodType
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; }
}
