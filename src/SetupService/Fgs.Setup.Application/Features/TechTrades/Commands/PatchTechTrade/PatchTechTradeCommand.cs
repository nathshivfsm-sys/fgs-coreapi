using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Commands.PatchTechTrade;

public sealed record PatchTechTradeCommand(long Id, TechTradePatchDto Dto)
    : IRequest<ApiResponse<TechTradeDetailDto>>;
