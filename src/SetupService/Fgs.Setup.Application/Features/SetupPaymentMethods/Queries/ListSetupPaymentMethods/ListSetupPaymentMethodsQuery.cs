using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.ListSetupPaymentMethods;

public sealed record ListSetupPaymentMethodsQuery(
    SetupListQuery Query, FgsSetupPaymentMethodListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupPaymentMethodSummaryDto>>>;
