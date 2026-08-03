using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Queries.ListFgsSetupPricingMatrixLabors;

public sealed record ListFgsSetupPricingMatrixLaborsQuery(SetupListQuery Query, FgsSetupPricingMatrixLaborListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsSetupPricingMatrixLaborSummaryDto>>>;
