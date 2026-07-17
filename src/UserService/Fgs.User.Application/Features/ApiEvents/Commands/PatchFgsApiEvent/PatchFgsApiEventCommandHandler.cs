using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiEvents;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiEvents.Commands.PatchFgsApiEvent;

public sealed class PatchFgsApiEventCommandHandler(
    IFgsApiEventWriteService writeService,
    ILogger<PatchFgsApiEventCommandHandler> logger)
    : IRequestHandler<PatchFgsApiEventCommand, ApiResponse<FgsApiEventDetailDto>>
{
    public async Task<ApiResponse<FgsApiEventDetailDto>> Handle(
        PatchFgsApiEventCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched API event {ApiEventId}", result.Id);
        return ApiResponse<FgsApiEventDetailDto>.Ok(result);
    }
}
