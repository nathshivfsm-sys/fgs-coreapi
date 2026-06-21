using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.UpdateFgsSalesActivityOutcome;

public sealed class UpdateFgsSalesActivityOutcomeCommandHandler(
    IFgsSalesActivityOutcomeWriteService writeService,
    ILogger<UpdateFgsSalesActivityOutcomeCommandHandler> logger)
    : IRequestHandler<UpdateFgsSalesActivityOutcomeCommand, ApiResponse<FgsSalesActivityOutcomeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityOutcomeDetailDto>> Handle(
        UpdateFgsSalesActivityOutcomeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated sales activity outcome {Id}", result.Id);
            return ApiResponse<FgsSalesActivityOutcomeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update sales activity outcome {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSalesActivityOutcomeDetailDto>(ex);
        }
    }
}
