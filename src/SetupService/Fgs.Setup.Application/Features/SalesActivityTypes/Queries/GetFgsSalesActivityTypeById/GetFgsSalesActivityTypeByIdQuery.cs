using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Queries.GetFgsSalesActivityTypeById;

public sealed record GetFgsSalesActivityTypeByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSalesActivityTypeDetailDto>>;
