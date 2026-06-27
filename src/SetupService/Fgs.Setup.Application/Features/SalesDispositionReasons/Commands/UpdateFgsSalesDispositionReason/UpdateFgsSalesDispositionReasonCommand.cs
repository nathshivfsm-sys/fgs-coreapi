using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.UpdateFgsSalesDispositionReason;

public sealed record UpdateFgsSalesDispositionReasonCommand(long Id, FgsSalesDispositionReasonUpdateDto Dto)
    : IRequest<ApiResponse<FgsSalesDispositionReasonDetailDto>>;
