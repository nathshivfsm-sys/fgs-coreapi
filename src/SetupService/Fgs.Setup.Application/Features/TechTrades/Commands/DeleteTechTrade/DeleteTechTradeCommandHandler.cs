using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TechTrades.Commands.DeleteTechTrade;

public sealed class DeleteTechTradeCommandHandler(
    ITechTradeWriteService writeService,
    ILogger<DeleteTechTradeCommandHandler> logger)
    : IRequestHandler<DeleteTechTradeCommand, ApiResponse<TechTradeDetailDto>>
{
    public async Task<ApiResponse<TechTradeDetailDto>> Handle(
        DeleteTechTradeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation(
                "Soft-deleted tech trade {TechTradeId} with code {TradeCode}",
                result.Id,
                result.TradeCode);

        return ApiResponse<TechTradeDetailDto>.Ok(result);
    }
}
