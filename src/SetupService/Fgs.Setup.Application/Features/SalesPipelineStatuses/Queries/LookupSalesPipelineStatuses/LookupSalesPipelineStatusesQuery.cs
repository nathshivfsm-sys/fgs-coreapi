using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.LookupSalesPipelineStatuses;

public sealed record LookupSalesPipelineStatusesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSalesPipelineStatusLookupDto>>>;
