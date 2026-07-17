using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiSecrets.Commands.RevokeFgsApiSecret;

public sealed record RevokeFgsApiSecretCommand(long Id) : IRequest<ApiResponse<FgsApiSecretDetailDto>>;
