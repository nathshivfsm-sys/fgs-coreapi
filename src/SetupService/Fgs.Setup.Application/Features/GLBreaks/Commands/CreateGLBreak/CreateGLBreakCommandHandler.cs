using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.GLBreaks.Commands.CreateGLBreak;

public sealed class CreateGLBreakCommandHandler(
    IGLBreakWriteService writeService,
    ILogger<CreateGLBreakCommandHandler> logger)
    : IRequestHandler<CreateGLBreakCommand, ApiResponse<GLBreakDetailDto>>
{
    public async Task<ApiResponse<GLBreakDetailDto>> Handle(
        CreateGLBreakCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
                "Created GL break {GLBreakId} with code {Code}",
                result.Id,
                result.Code);

        return ApiResponse<GLBreakDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
