using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.DeleteCustomerPaymentInformation;

public sealed record DeleteCustomerPaymentInformationCommand(long Id)
    : IRequest<ApiResponse<CustomerPaymentInformationGridDto>>;
