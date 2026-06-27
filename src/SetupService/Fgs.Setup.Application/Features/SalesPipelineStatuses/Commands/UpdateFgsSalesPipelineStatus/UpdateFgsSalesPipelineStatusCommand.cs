using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.UpdateFgsSalesPipelineStatus;

public sealed record UpdateFgsSalesPipelineStatusCommand(long Id, FgsSalesPipelineStatusUpdateDto Dto)
    : IRequest<ApiResponse<FgsSalesPipelineStatusDetailDto>>;
