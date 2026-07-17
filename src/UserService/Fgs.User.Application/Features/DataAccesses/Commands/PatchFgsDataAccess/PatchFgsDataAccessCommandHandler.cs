using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.DataAccesses;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.DataAccesses.Commands.PatchFgsDataAccess;

public sealed class PatchFgsDataAccessCommandHandler(
    IFgsDataAccessWriteService writeService,
    ILogger<PatchFgsDataAccessCommandHandler> logger)
    : IRequestHandler<PatchFgsDataAccessCommand, ApiResponse<FgsDataAccessDetailDto>>
{
    public async Task<ApiResponse<FgsDataAccessDetailDto>> Handle(
        PatchFgsDataAccessCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched data access {DataAccessId}", result.Id);
        return ApiResponse<FgsDataAccessDetailDto>.Ok(result);
    }
}
