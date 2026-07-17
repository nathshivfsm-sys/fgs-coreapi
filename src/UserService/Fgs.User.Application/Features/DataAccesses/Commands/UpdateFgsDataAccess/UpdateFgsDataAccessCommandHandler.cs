using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.DataAccesses;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.DataAccesses.Commands.UpdateFgsDataAccess;

public sealed class UpdateFgsDataAccessCommandHandler(
    IFgsDataAccessWriteService writeService,
    ILogger<UpdateFgsDataAccessCommandHandler> logger)
    : IRequestHandler<UpdateFgsDataAccessCommand, ApiResponse<FgsDataAccessDetailDto>>
{
    public async Task<ApiResponse<FgsDataAccessDetailDto>> Handle(
        UpdateFgsDataAccessCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated data access {DataAccessId}", result.Id);
        return ApiResponse<FgsDataAccessDetailDto>.Ok(result);
    }
}
