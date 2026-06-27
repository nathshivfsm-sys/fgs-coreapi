using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Commands.UpdateTechTrade;

public sealed record UpdateTechTradeCommand(long Id, TechTradeUpdateDto Dto)
    : IRequest<ApiResponse<TechTradeDetailDto>>;
