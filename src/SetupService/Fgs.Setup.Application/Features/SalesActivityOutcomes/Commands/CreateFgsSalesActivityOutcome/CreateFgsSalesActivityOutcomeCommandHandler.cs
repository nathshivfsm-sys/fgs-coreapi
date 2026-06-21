using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.CreateFgsSalesActivityOutcome;

public sealed class CreateFgsSalesActivityOutcomeCommandHandler(
    IFgsSalesActivityOutcomeWriteService writeService,
    ILogger<CreateFgsSalesActivityOutcomeCommandHandler> logger)
    : IRequestHandler<CreateFgsSalesActivityOutcomeCommand, ApiResponse<FgsSalesActivityOutcomeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityOutcomeDetailDto>> Handle(
        CreateFgsSalesActivityOutcomeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created sales activity outcome {Id} with code {OutcomeCode}", result.Id, result.OutcomeCode);
            return ApiResponse<FgsSalesActivityOutcomeDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create sales activity outcome");
            return CatalogCrudExceptionMapper.MapException<FgsSalesActivityOutcomeDetailDto>(ex);
        }
    }
}
