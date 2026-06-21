using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Commands.UpdateFgsSalesActivityType;

public sealed record UpdateFgsSalesActivityTypeCommand(long Id, FgsSalesActivityTypeUpdateDto Dto)
    : IRequest<ApiResponse<FgsSalesActivityTypeDetailDto>>;
