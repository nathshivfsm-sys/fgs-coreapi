using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.PatchFgsSalesDispositionReason;

public sealed class PatchFgsSalesDispositionReasonCommandHandler(
    IFgsSalesDispositionReasonWriteService writeService,
    ILogger<PatchFgsSalesDispositionReasonCommandHandler> logger)
    : IRequestHandler<PatchFgsSalesDispositionReasonCommand, ApiResponse<FgsSalesDispositionReasonDetailDto>>
{
    public async Task<ApiResponse<FgsSalesDispositionReasonDetailDto>> Handle(
        PatchFgsSalesDispositionReasonCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd sales disposition reason {Id}", result.Id);
            return ApiResponse<FgsSalesDispositionReasonDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch sales disposition reason {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSalesDispositionReasonDetailDto>(ex);
        }
    }
}
