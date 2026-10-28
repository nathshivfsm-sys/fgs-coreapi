using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.GetCustomerPaymentInformationById;

public sealed record GetCustomerPaymentInformationByIdQuery(long Id)
    : IRequest<ApiResponse<CustomerPaymentInformationGridDto>>;
