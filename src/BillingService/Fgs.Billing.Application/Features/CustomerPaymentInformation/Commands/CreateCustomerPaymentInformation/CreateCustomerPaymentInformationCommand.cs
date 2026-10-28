using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.CreateCustomerPaymentInformation;

public sealed record CreateCustomerPaymentInformationCommand(CustomerPaymentInformationCreateDto Dto)
    : IRequest<ApiResponse<CustomerPaymentInformationGridDto>>;
