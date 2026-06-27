using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Commands.CreateTechTrade;

public sealed record CreateTechTradeCommand(TechTradeCreateDto Dto)
    : IRequest<ApiResponse<TechTradeDetailDto>>;
