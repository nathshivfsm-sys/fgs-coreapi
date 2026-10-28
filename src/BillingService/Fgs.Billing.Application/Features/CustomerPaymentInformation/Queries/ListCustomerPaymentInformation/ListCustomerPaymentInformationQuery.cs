using Fgs.Billing.Application.Common.BillingCrud;
using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.ListCustomerPaymentInformation;

public sealed record ListCustomerPaymentInformationQuery(
    long ArCustomerId,
    BillingListQuery Query,
    bool IsActive = true) : IRequest<ApiResponse<CustomerPaymentInformationListDto>>;
