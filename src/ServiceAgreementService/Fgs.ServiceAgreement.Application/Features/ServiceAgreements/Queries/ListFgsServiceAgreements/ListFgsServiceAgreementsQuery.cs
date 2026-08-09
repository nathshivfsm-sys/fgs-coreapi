using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.ServiceAgreement.Application.Common.ServiceAgreementCrud;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;
using MediatR;

namespace Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Queries.ListFgsServiceAgreements;

public sealed record ListFgsServiceAgreementsQuery(
    ServiceAgreementListQuery Query,
    FgsServiceAgreementListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsServiceAgreementSummaryDto>>>;
