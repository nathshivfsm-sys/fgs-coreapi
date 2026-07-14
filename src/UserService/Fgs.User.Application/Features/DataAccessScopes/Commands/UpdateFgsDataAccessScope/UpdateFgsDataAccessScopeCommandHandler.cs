using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.DataAccessScopes;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.DataAccessScopes.Commands.UpdateFgsDataAccessScope;

public sealed class UpdateFgsDataAccessScopeCommandHandler(
    IFgsDataAccessScopeWriteService writeService,
    ILogger<UpdateFgsDataAccessScopeCommandHandler> logger)
    : IRequestHandler<UpdateFgsDataAccessScopeCommand, ApiResponse<FgsDataAccessScopeDetailDto>>
{
    public async Task<ApiResponse<FgsDataAccessScopeDetailDto>> Handle(
        UpdateFgsDataAccessScopeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated data access scope {ScopeId}", result.Id);
        return ApiResponse<FgsDataAccessScopeDetailDto>.Ok(result);
    }
}
