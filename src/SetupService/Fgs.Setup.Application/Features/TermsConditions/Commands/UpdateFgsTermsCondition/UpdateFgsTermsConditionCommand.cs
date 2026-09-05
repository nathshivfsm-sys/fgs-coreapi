using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TermsConditions.Commands.UpdateFgsTermsCondition;

public sealed record UpdateFgsTermsConditionCommand(long Id, FgsTermsConditionUpdateDto Dto)
    : IRequest<ApiResponse<FgsTermsConditionDetailDto>>;
