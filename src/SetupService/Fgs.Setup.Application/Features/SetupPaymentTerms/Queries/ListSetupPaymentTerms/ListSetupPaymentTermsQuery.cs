using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Queries.ListSetupPaymentTerms;

public sealed record ListSetupPaymentTermsQuery(
    SetupListQuery Query, FgsSetupPaymentTermListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupPaymentTermSummaryDto>>>;
