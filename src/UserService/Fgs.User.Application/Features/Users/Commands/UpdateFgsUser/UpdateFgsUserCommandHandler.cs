using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Users.Commands.UpdateFgsUser;

public sealed class UpdateFgsUserCommandHandler(
    IFgsUserWriteService writeService,
    ILogger<UpdateFgsUserCommandHandler> logger)
    : IRequestHandler<UpdateFgsUserCommand, ApiResponse<FgsUserDetailDto>>
{
    public async Task<ApiResponse<FgsUserDetailDto>> Handle(
        UpdateFgsUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated user {UserId}", result.Id);
        return ApiResponse<FgsUserDetailDto>.Ok(result);
    }
}
