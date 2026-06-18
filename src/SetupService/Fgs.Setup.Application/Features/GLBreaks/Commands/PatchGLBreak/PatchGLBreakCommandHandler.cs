using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.GLBreaks.Commands.PatchGLBreak;

public sealed class PatchGLBreakCommandHandler(
    IGLBreakWriteService writeService,
    ILogger<PatchGLBreakCommandHandler> logger)
    : IRequestHandler<PatchGLBreakCommand, ApiResponse<GLBreakDetailDto>>
{
    public async Task<ApiResponse<GLBreakDetailDto>> Handle(
        PatchGLBreakCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patched GL break {GLBreakId}", result.Id);
            return ApiResponse<GLBreakDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch GL break {GLBreakId}", request.Id);
            return CatalogCrudExceptionMapper.MapException<GLBreakDetailDto>(ex);
        }
    }
}
