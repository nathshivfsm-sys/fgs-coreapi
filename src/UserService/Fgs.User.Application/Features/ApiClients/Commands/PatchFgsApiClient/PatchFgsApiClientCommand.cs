using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiClients.Commands.PatchFgsApiClient;

public sealed record PatchFgsApiClientCommand(long Id, FgsApiClientPatchDto Dto)
    : IRequest<ApiResponse<FgsApiClientDetailDto>>;
