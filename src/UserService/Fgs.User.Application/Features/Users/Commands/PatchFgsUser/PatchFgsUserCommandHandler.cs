using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Users.Commands.PatchFgsUser;

public sealed class PatchFgsUserCommandHandler(
    IFgsUserWriteService writeService,
    ILogger<PatchFgsUserCommandHandler> logger)
    : IRequestHandler<PatchFgsUserCommand, ApiResponse<FgsUserDetailDto>>
{
    public async Task<ApiResponse<FgsUserDetailDto>> Handle(
        PatchFgsUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched user {UserId}", result.Id);
        return ApiResponse<FgsUserDetailDto>.Ok(result);
    }
}
