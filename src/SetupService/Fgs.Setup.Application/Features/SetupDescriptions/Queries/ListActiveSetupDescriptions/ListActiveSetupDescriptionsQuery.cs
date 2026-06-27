using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Queries.ListActiveSetupDescriptions;

public sealed record ListActiveSetupDescriptionsQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSetupDescriptionListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsSetupDescriptionSummaryDto>>>;
