using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiSecrets.Commands.CreateFgsApiSecret;

public sealed record CreateFgsApiSecretCommand(FgsApiSecretCreateDto Dto)
    : IRequest<ApiResponse<FgsApiSecretCreateResultDto>>;
