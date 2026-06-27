using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TechTrades.Commands.UpdateTechTrade;

public sealed class UpdateTechTradeCommandHandler(
    ITechTradeWriteService writeService,
    ILogger<UpdateTechTradeCommandHandler> logger)
    : IRequestHandler<UpdateTechTradeCommand, ApiResponse<TechTradeDetailDto>>
{
    public async Task<ApiResponse<TechTradeDetailDto>> Handle(
        UpdateTechTradeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation(
                "Updated tech trade {TechTradeId} with code {TradeCode}",
                result.Id,
                result.TradeCode);

        return ApiResponse<TechTradeDetailDto>.Ok(result);
    }
}
