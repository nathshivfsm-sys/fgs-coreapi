using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.UpdateCustomerPaymentInformation;

public sealed record UpdateCustomerPaymentInformationCommand(long Id, CustomerPaymentInformationUpdateDto Dto)
    : IRequest<ApiResponse<CustomerPaymentInformationGridDto>>;
