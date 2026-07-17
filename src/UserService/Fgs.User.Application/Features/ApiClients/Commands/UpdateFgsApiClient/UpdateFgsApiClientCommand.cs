using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiClients.Commands.UpdateFgsApiClient;

public sealed record UpdateFgsApiClientCommand(long Id, FgsApiClientUpdateDto Dto)
    : IRequest<ApiResponse<FgsApiClientDetailDto>>;
