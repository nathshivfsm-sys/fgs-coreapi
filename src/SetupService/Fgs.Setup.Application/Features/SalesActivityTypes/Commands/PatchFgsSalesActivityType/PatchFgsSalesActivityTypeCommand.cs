using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Commands.PatchFgsSalesActivityType;

public sealed record PatchFgsSalesActivityTypeCommand(long Id, FgsSalesActivityTypePatchDto Dto)
    : IRequest<ApiResponse<FgsSalesActivityTypeDetailDto>>;
