using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiSecrets.Queries.GetFgsApiSecretById;

public sealed record GetFgsApiSecretByIdQuery(long Id) : IRequest<ApiResponse<FgsApiSecretDetailDto>>;
