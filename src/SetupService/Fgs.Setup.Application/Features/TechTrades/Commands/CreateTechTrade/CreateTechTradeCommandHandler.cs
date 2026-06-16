using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
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
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation(
                "Created tech trade {TechTradeId} with code {TradeCode} for tenant {TenantId} company {CompanyId}",
                result.Id,
                result.TradeCode,
                result.TenantId,
                result.CompanyId);

            return ApiResponse<TechTradeDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create tech trade with code {TradeCode}", request.Dto.TradeCode);
            return CatalogCrudExceptionMapper.MapException<TechTradeDetailDto>(ex);
        }
    }
}
