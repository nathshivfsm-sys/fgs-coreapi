using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation;
using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.GetCustomerPaymentInformationById;

public sealed class GetCustomerPaymentInformationByIdQueryHandler(ICustomerPaymentInformationReadRepository readRepository)
    : IRequestHandler<GetCustomerPaymentInformationByIdQuery, ApiResponse<CustomerPaymentInformationGridDto>>
{
    public async Task<ApiResponse<CustomerPaymentInformationGridDto>> Handle(
        GetCustomerPaymentInformationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var card = await readRepository.GetCardAsync(request.Id, cancellationToken);
        if (card is null)
        {
            return CustomerPaymentInformationResponses.NotFound<CustomerPaymentInformationGridDto>();
        }

        return ApiResponse<CustomerPaymentInformationGridDto>.Ok(CustomerPaymentInformationGrid.FromCard(card));
    }
}
