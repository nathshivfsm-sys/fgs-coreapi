using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Queries.ListFgsEntityDefaultTermsConditions;

public sealed record ListFgsEntityDefaultTermsConditionsQuery(
    SetupListQuery Query,
    FgsEntityDefaultTermsConditionListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsEntityDefaultTermsConditionSummaryDto>>>;
