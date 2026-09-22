using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.Users.Commands.SetFgsUserAccess;

public sealed record SetFgsUserAccessCommand(Guid UserId, bool IsActive)
    : IRequest<ApiResponse<object>>;
