using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.UpdateFgsSalesActivityOutcome;

public sealed record UpdateFgsSalesActivityOutcomeCommand(long Id, FgsSalesActivityOutcomeUpdateDto Dto)
    : IRequest<ApiResponse<FgsSalesActivityOutcomeDetailDto>>;
