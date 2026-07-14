using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RoleDataAccesses.Commands.DeleteFgsRoleDataAccess;

public sealed class DeleteFgsRoleDataAccessCommandHandler(
    IFgsRoleDataAccessWriteService writeService,
    ILogger<DeleteFgsRoleDataAccessCommandHandler> logger)
    : IRequestHandler<DeleteFgsRoleDataAccessCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        DeleteFgsRoleDataAccessCommand request,
        CancellationToken cancellationToken)
    {
        await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Removed role data access assignment {Id}", request.Id);
        return ApiResponse<object>.Ok(new object());
    }
}
