using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TechTrades.Commands.PatchTechTrade;

public sealed class PatchTechTradeCommandHandler(
    ITechTradeWriteService writeService,
    ILogger<PatchTechTradeCommandHandler> logger)
    : IRequestHandler<PatchTechTradeCommand, ApiResponse<TechTradeDetailDto>>
{
    public async Task<ApiResponse<TechTradeDetailDto>> Handle(
        PatchTechTradeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patched tech trade {TechTradeId}", result.Id);

            return ApiResponse<TechTradeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch tech trade {TechTradeId}", request.Id);
            return CatalogCrudExceptionMapper.MapException<TechTradeDetailDto>(ex);
        }
    }
}
