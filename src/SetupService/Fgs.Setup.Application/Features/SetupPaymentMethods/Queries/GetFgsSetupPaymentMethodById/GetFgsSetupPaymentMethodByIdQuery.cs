using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.GetFgsSetupPaymentMethodById;

public sealed record GetFgsSetupPaymentMethodByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupPaymentMethodDetailDto>>;
