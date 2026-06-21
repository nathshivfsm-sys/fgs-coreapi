using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Commands.DeleteFgsSalesActivityType;

public sealed record DeleteFgsSalesActivityTypeCommand(long Id)
    : IRequest<ApiResponse<FgsSalesActivityTypeDetailDto>>;
