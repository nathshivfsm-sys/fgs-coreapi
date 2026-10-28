using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;

namespace Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;

public interface ICustomerPaymentInformationWriteService
{
    Task<CustomerPaymentInformationGridDto> CreateAsync(
        CustomerPaymentInformationCreateModel model,
        CancellationToken cancellationToken = default);

    Task<CustomerPaymentInformationGridDto?> UpdateAsync(
        long id,
        CustomerPaymentInformationUpdateModel model,
        CancellationToken cancellationToken = default);

    Task<CustomerPaymentInformationGridDto?> SoftDeleteAsync(
        long id,
        CancellationToken cancellationToken = default);
}
