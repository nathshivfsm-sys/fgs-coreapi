using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Queries.GetAuthMe;

public sealed record GetAuthMeQuery : IRequest<ApiResponse<FgsAuthMeDto>>;
