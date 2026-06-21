using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.PatchFgsSetupPaymentMethod;

public sealed record PatchFgsSetupPaymentMethodCommand(long Id, FgsSetupPaymentMethodPatchDto Dto)
    : IRequest<ApiResponse<FgsSetupPaymentMethodDetailDto>>;
