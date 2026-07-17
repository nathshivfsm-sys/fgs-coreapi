using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiEvents;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiEvents.Commands.CreateFgsApiEvent;

public sealed class CreateFgsApiEventCommandHandler(
    IFgsApiEventWriteService writeService,
    ILogger<CreateFgsApiEventCommandHandler> logger)
    : IRequestHandler<CreateFgsApiEventCommand, ApiResponse<FgsApiEventDetailDto>>
{
    public async Task<ApiResponse<FgsApiEventDetailDto>> Handle(
        CreateFgsApiEventCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created API event {ApiEventId} with code {EventCode}",
            result.Id,
            result.EventCode);
        return ApiResponse<FgsApiEventDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
