using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiClients.Queries.ListFgsApiClients;

public sealed record ListFgsApiClientsQuery(
    IdentityListQuery Query,
    FgsApiClientListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsApiClientSummaryDto>>>;
