using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Commands.DeleteTechTrade;

public sealed record DeleteTechTradeCommand(long Id)
    : IRequest<ApiResponse<TechTradeDetailDto>>;
