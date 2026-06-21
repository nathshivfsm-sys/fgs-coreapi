using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.DeleteFgsSetupPaymentMethod;

public sealed record DeleteFgsSetupPaymentMethodCommand(long Id)
    : IRequest<ApiResponse<FgsSetupPaymentMethodDetailDto>>;
