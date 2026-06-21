using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.UpdateFgsSalesDispositionReason;

public sealed class UpdateFgsSalesDispositionReasonCommandHandler(
    IFgsSalesDispositionReasonWriteService writeService,
    ILogger<UpdateFgsSalesDispositionReasonCommandHandler> logger)
    : IRequestHandler<UpdateFgsSalesDispositionReasonCommand, ApiResponse<FgsSalesDispositionReasonDetailDto>>
{
    public async Task<ApiResponse<FgsSalesDispositionReasonDetailDto>> Handle(
        UpdateFgsSalesDispositionReasonCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated sales disposition reason {Id}", result.Id);
            return ApiResponse<FgsSalesDispositionReasonDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update sales disposition reason {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSalesDispositionReasonDetailDto>(ex);
        }
    }
}
