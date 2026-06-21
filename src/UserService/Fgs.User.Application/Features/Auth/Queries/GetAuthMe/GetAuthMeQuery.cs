using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Queries.GetAuthMe;

public sealed record GetAuthMeQuery : IRequest<ApiResponse<AuthMeDto>>;
