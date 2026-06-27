using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.PatchFgsSalesDispositionReason;

public sealed record PatchFgsSalesDispositionReasonCommand(long Id, FgsSalesDispositionReasonPatchDto Dto)
    : IRequest<ApiResponse<FgsSalesDispositionReasonDetailDto>>;
