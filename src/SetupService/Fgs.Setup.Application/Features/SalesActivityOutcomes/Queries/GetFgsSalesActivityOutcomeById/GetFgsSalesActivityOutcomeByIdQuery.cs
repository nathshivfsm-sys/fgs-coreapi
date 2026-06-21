using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.GetFgsSalesActivityOutcomeById;

public sealed record GetFgsSalesActivityOutcomeByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSalesActivityOutcomeDetailDto>>;
