using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.GLBreaks.Commands.UpdateGLBreak;

public sealed class UpdateGLBreakCommandHandler(
    IGLBreakWriteService writeService,
    ILogger<UpdateGLBreakCommandHandler> logger)
    : IRequestHandler<UpdateGLBreakCommand, ApiResponse<GLBreakDetailDto>>
{
    public async Task<ApiResponse<GLBreakDetailDto>> Handle(
        UpdateGLBreakCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated GL break {GLBreakId}", result.Id);
            return ApiResponse<GLBreakDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update GL break {GLBreakId}", request.Id);
            return CatalogCrudExceptionMapper.MapException<GLBreakDetailDto>(ex);
        }
    }
}
