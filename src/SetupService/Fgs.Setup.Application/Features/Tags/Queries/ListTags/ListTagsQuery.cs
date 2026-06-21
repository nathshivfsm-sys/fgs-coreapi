using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Queries.ListTags;

public sealed record ListTagsQuery(
    SetupListQuery Query, FgsTagListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsTagSummaryDto>>>;
