using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.PatchFgsSalesActivityOutcome;

public sealed record PatchFgsSalesActivityOutcomeCommand(long Id, FgsSalesActivityOutcomePatchDto Dto)
    : IRequest<ApiResponse<FgsSalesActivityOutcomeDetailDto>>;
