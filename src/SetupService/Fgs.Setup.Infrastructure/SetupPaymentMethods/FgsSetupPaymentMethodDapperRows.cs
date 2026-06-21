using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;

namespace Fgs.Setup.Infrastructure.SetupPaymentMethods;

internal sealed class FgsSetupPaymentMethodSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string DisplayName { get; set; }
    public int SortOrder { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsCustomerPortalVisible { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSetupPaymentMethodSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            DisplayName,
            SortOrder,
            IsMobileVisible,
            IsCustomerPortalVisible,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSetupPaymentMethodDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string DisplayName { get; set; }
    public int SortOrder { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsCustomerPortalVisible { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSetupPaymentMethodDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            DisplayName,
            SortOrder,
            IsMobileVisible,
            IsCustomerPortalVisible,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSetupPaymentMethodLookupRow
{
    public long Id { get; set; }
    public string DisplayName { get; set; }
    public int SortOrder { get; set; }

    public FgsSetupPaymentMethodLookupDto ToDto() => new(Id,
            DisplayName,
            SortOrder);
}
