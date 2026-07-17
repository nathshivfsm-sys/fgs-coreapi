using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiSecrets.Commands.PatchFgsApiSecret;

public sealed record PatchFgsApiSecretCommand(long Id, FgsApiSecretPatchDto Dto)
    : IRequest<ApiResponse<FgsApiSecretDetailDto>>;
