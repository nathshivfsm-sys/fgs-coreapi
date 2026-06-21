using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.PatchFgsSalesActivityOutcome;

public sealed class PatchFgsSalesActivityOutcomeCommandHandler(
    IFgsSalesActivityOutcomeWriteService writeService,
    ILogger<PatchFgsSalesActivityOutcomeCommandHandler> logger)
    : IRequestHandler<PatchFgsSalesActivityOutcomeCommand, ApiResponse<FgsSalesActivityOutcomeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityOutcomeDetailDto>> Handle(
        PatchFgsSalesActivityOutcomeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd sales activity outcome {Id}", result.Id);
            return ApiResponse<FgsSalesActivityOutcomeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch sales activity outcome {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSalesActivityOutcomeDetailDto>(ex);
        }
    }
}
