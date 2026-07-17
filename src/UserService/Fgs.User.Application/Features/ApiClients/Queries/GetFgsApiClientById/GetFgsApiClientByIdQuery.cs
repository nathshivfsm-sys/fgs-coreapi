using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiClients.Queries.GetFgsApiClientById;

public sealed record GetFgsApiClientByIdQuery(long Id) : IRequest<ApiResponse<FgsApiClientDetailDto>>;
