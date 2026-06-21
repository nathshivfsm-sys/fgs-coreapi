using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.CreateFgsSalesActivityOutcome;

public sealed record CreateFgsSalesActivityOutcomeCommand(FgsSalesActivityOutcomeCreateDto Dto)
    : IRequest<ApiResponse<FgsSalesActivityOutcomeDetailDto>>;
