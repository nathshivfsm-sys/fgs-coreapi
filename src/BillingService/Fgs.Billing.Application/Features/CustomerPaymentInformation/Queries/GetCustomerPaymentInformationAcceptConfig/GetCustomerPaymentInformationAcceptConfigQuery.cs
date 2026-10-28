using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.GetCustomerPaymentInformationAcceptConfig;

public sealed record GetCustomerPaymentInformationAcceptConfigQuery
    : IRequest<ApiResponse<CustomerPaymentInformationAcceptConfigDto>>;
