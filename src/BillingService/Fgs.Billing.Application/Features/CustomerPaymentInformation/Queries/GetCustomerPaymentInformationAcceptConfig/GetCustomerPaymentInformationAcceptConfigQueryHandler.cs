using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation;
using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.GetCustomerPaymentInformationAcceptConfig;

public sealed class GetCustomerPaymentInformationAcceptConfigQueryHandler(ICompanyAuthorizeNetSettings settings)
    : IRequestHandler<GetCustomerPaymentInformationAcceptConfigQuery, ApiResponse<CustomerPaymentInformationAcceptConfigDto>>
{
    public async Task<ApiResponse<CustomerPaymentInformationAcceptConfigDto>> Handle(
        GetCustomerPaymentInformationAcceptConfigQuery request,
        CancellationToken cancellationToken)
    {
        var lookup = await settings.GetAsync(cancellationToken);
        if (!CustomerPaymentInformationResponses.TryGetSettings<CustomerPaymentInformationAcceptConfigDto>(
                lookup,
                out var merchant,
                out var failure))
        {
            return failure!;
        }

        return ApiResponse<CustomerPaymentInformationAcceptConfigDto>.Ok(
            new CustomerPaymentInformationAcceptConfigDto(
                merchant.ApiLoginId,
                merchant.PublicClientKey,
                AuthorizeNetEndpoints.AcceptJsUrl(merchant.UseSandbox)));
    }
}
