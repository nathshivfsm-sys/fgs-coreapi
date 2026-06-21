using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.DeleteFgsSalesActivityOutcome;

public sealed record DeleteFgsSalesActivityOutcomeCommand(long Id)
    : IRequest<ApiResponse<FgsSalesActivityOutcomeDetailDto>>;
