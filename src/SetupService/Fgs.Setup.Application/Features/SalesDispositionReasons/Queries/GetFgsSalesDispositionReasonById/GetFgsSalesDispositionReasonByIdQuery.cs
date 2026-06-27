using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Queries.GetFgsSalesDispositionReasonById;

public sealed record GetFgsSalesDispositionReasonByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSalesDispositionReasonDetailDto>>;
