using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Queries.GetTechTradeById;

public sealed record GetTechTradeByIdQuery(long Id)
    : IRequest<ApiResponse<TechTradeDetailDto>>;
