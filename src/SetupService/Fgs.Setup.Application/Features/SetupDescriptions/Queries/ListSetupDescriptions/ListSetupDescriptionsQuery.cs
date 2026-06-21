using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Queries.ListSetupDescriptions;

public sealed record ListSetupDescriptionsQuery(
    SetupListQuery Query, FgsSetupDescriptionListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupDescriptionSummaryDto>>>;
