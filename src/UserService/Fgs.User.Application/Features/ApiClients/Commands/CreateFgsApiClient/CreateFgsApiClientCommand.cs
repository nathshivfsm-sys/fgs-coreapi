using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiClients.Commands.CreateFgsApiClient;

public sealed record CreateFgsApiClientCommand(FgsApiClientCreateDto Dto)
    : IRequest<ApiResponse<FgsApiClientDetailDto>>;
