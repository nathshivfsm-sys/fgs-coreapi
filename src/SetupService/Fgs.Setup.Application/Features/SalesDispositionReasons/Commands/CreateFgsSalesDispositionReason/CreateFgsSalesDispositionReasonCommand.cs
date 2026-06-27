using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.CreateFgsSalesDispositionReason;

public sealed record CreateFgsSalesDispositionReasonCommand(FgsSalesDispositionReasonCreateDto Dto)
    : IRequest<ApiResponse<FgsSalesDispositionReasonDetailDto>>;
