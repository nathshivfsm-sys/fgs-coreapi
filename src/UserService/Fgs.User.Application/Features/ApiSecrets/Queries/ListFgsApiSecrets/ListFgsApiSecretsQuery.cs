using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiSecrets.Queries.ListFgsApiSecrets;

public sealed record ListFgsApiSecretsQuery(
    IdentityListQuery Query,
    FgsApiSecretListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsApiSecretSummaryDto>>>;
