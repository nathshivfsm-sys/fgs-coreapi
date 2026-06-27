using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.PatchFgsSalesPipelineStatus;

public sealed record PatchFgsSalesPipelineStatusCommand(long Id, FgsSalesPipelineStatusPatchDto Dto)
    : IRequest<ApiResponse<FgsSalesPipelineStatusDetailDto>>;
