using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.GLBreaks.Commands.DeleteGLBreak;

public sealed class DeleteGLBreakCommandHandler(
    IGLBreakWriteService writeService,
    ILogger<DeleteGLBreakCommandHandler> logger)
    : IRequestHandler<DeleteGLBreakCommand, ApiResponse<GLBreakDetailDto>>
{
    public async Task<ApiResponse<GLBreakDetailDto>> Handle(
        DeleteGLBreakCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted GL break {GLBreakId}", result.Id);
        return ApiResponse<GLBreakDetailDto>.Ok(result);
    }
}
