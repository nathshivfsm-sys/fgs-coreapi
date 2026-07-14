using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiEvents;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiEvents.Commands.UpdateFgsApiEvent;

public sealed class UpdateFgsApiEventCommandHandler(
    IFgsApiEventWriteService writeService,
    ILogger<UpdateFgsApiEventCommandHandler> logger)
    : IRequestHandler<UpdateFgsApiEventCommand, ApiResponse<FgsApiEventDetailDto>>
{
    public async Task<ApiResponse<FgsApiEventDetailDto>> Handle(
        UpdateFgsApiEventCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated API event {ApiEventId}", result.Id);
        return ApiResponse<FgsApiEventDetailDto>.Ok(result);
    }
}
