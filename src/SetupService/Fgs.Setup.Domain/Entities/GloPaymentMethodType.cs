namespace Fgs.Setup.Domain.Entities;

public class GloPaymentMethodType : GloEntityBase
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int SortOrder { get; set; }
}
