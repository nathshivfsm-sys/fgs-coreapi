using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.DeleteFgsSalesDispositionReason;

public sealed record DeleteFgsSalesDispositionReasonCommand(long Id)
    : IRequest<ApiResponse<FgsSalesDispositionReasonDetailDto>>;
