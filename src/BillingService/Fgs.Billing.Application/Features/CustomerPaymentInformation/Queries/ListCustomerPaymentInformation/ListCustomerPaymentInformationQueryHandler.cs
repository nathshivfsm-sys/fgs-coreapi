using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;
using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.ListCustomerPaymentInformation;

public sealed class ListCustomerPaymentInformationQueryHandler(ICustomerPaymentInformationReadRepository readRepository)
    : IRequestHandler<ListCustomerPaymentInformationQuery, ApiResponse<CustomerPaymentInformationListDto>>
{
    public async Task<ApiResponse<CustomerPaymentInformationListDto>> Handle(
        ListCustomerPaymentInformationQuery request,
        CancellationToken cancellationToken)
    {
        if (request.ArCustomerId <= 0)
        {
            return ApiResponse<CustomerPaymentInformationListDto>.Fail(
                ["arCustomerId is required."],
                ApiStatusCodes.BadRequest);
        }

        var page = await readRepository.ListAsync(
            request.ArCustomerId,
            request.Query,
            request.IsActive,
            cancellationToken);

        return ApiResponse<CustomerPaymentInformationListDto>.Ok(page);
    }
}
