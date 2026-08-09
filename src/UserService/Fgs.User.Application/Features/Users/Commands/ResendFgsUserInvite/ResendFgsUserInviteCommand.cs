using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Users.Commands.ResendFgsUserInvite;

public sealed record ResendFgsUserInviteCommand(Guid Id) : IRequest<ApiResponse<FgsUserDetailDto>>;
