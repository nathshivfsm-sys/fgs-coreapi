using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.UserRoles;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.UserRoles.Commands.DeleteFgsUserRole;

public sealed class DeleteFgsUserRoleCommandHandler(
    IFgsUserRoleWriteService writeService,
    ILogger<DeleteFgsUserRoleCommandHandler> logger)
    : IRequestHandler<DeleteFgsUserRoleCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        DeleteFgsUserRoleCommand request,
        CancellationToken cancellationToken)
    {
        await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Removed user role assignment {Id}", request.Id);
        return ApiResponse<object>.Ok(new object());
    }
}
