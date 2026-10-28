using Fgs.Billing.Application.Common.BillingCrud;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;

namespace Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;

public interface ICustomerPaymentInformationReadRepository
{
    Task<CustomerPaymentInformationCard?> GetCardAsync(long id, CancellationToken cancellationToken = default);

    Task<string?> FindCustomerProfileIdAsync(long arCustomerId, CancellationToken cancellationToken = default);

    Task<CustomerPaymentInformationListDto> ListAsync(
        long arCustomerId,
        BillingListQuery query,
        bool isActive,
        CancellationToken cancellationToken = default);
}
