using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.GetFgsSalesPipelineStatusById;

public sealed record GetFgsSalesPipelineStatusByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSalesPipelineStatusDetailDto>>;
