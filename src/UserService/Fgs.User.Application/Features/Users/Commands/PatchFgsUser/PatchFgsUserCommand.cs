using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Users.Commands.PatchFgsUser;

public sealed record PatchFgsUserCommand(Guid Id, FgsUserPatchDto Dto) : IRequest<ApiResponse<FgsUserDetailDto>>;
