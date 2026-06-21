using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.DeleteFgsSalesPipelineStatus;

public sealed record DeleteFgsSalesPipelineStatusCommand(long Id)
    : IRequest<ApiResponse<FgsSalesPipelineStatusDetailDto>>;
