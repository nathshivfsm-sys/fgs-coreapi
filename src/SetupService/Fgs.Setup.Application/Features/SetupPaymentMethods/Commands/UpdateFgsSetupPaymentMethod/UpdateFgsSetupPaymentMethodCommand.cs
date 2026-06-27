using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.UpdateFgsSetupPaymentMethod;

public sealed record UpdateFgsSetupPaymentMethodCommand(long Id, FgsSetupPaymentMethodUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupPaymentMethodDetailDto>>;
