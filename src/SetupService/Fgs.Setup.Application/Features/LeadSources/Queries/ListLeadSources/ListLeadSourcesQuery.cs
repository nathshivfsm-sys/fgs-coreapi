using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Queries.ListLeadSources;

public sealed record ListLeadSourcesQuery(
    SetupListQuery Query, LeadSourceListFilters Filters)
    : IRequest<ApiResponse<PagedResult<LeadSourceSummaryDto>>>;
