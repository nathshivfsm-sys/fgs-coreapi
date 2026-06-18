using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Queries.ValidateAuthUser;

public sealed record ValidateAuthUserQuery : IRequest<ApiResponse<object>>;
