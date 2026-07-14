using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiClients.Queries.LookupFgsApiClients;

public sealed record LookupFgsApiClientsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsApiClientLookupDto>>>;
