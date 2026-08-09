using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Users.Commands.UpdateFgsUser;

public sealed record UpdateFgsUserCommand(Guid Id, FgsUserUpdateDto Dto) : IRequest<ApiResponse<FgsUserDetailDto>>;
