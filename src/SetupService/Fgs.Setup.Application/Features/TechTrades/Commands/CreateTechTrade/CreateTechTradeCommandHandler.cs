using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TechTrades.Commands.CreateTechTrade;

public sealed class CreateTechTradeCommandHandler(
    ITechTradeWriteService writeService,
    ILogger<CreateTechTradeCommandHandler> logger)
    : IRequestHandler<CreateTechTradeCommand, ApiResponse<TechTradeDetailDto>>
{
    public async Task<ApiResponse<TechTradeDetailDto>> Handle(
        CreateTechTradeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
                "Created tech trade {TechTradeId} with code {TradeCode}",
                result.Id,
                result.TradeCode);

        return ApiResponse<TechTradeDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
