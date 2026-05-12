namespace Fgs.User.Domain.Entities;

public class GloPaymentMethodType : GloActiveOnlyEntityBase
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int SortOrder { get; set; }
}
