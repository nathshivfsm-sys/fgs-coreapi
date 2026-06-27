using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.CreateFgsSalesPipelineStatus;

public sealed record CreateFgsSalesPipelineStatusCommand(FgsSalesPipelineStatusCreateDto Dto)
    : IRequest<ApiResponse<FgsSalesPipelineStatusDetailDto>>;
