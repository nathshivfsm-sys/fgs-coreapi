using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;

namespace Fgs.Setup.Infrastructure.Entities.SetupPaymentMethods;

internal sealed class FgsSetupPaymentMethodSummaryRow
{
    public long Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public int SortOrder { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsCustomerPortalVisible { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupPaymentMethodSummaryDto ToDto() =>
        new(
            Id,
            DisplayName,
            SortOrder,
            IsMobileVisible,
            IsCustomerPortalVisible,
            IsActive);
}

internal sealed class FgsSetupPaymentMethodDetailRow
{
    public long Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public int SortOrder { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsCustomerPortalVisible { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupPaymentMethodDetailDto ToDto() =>
        new(
            Id,
            DisplayName,
            SortOrder,
            IsMobileVisible,
            IsCustomerPortalVisible,
            IsActive);
}

internal sealed class FgsSetupPaymentMethodLookupRow
{
    public long Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public int SortOrder { get; set; }

    public FgsSetupPaymentMethodLookupDto ToDto() => new(Id,
            DisplayName,
            SortOrder);
}
