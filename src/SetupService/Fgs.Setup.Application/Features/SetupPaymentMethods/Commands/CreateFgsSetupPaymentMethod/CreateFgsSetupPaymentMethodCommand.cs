using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.CreateFgsSetupPaymentMethod;

public sealed record CreateFgsSetupPaymentMethodCommand(FgsSetupPaymentMethodCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupPaymentMethodDetailDto>>;
