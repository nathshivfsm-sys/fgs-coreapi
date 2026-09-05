using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TermsConditions.Commands.PatchFgsTermsCondition;

public sealed record PatchFgsTermsConditionCommand(long Id, FgsTermsConditionPatchDto Dto)
    : IRequest<ApiResponse<FgsTermsConditionDetailDto>>;
