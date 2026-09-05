using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TermsConditions.Queries.ListFgsTermsConditions;

public sealed record ListFgsTermsConditionsQuery(
    SetupListQuery Query,
    FgsTermsConditionListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsTermsConditionSummaryDto>>>;
