using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.PatchFgsEntityDefaultTermsCondition;

public sealed record PatchFgsEntityDefaultTermsConditionCommand(long Id, FgsEntityDefaultTermsConditionPatchDto Dto)
    : IRequest<ApiResponse<FgsEntityDefaultTermsConditionDetailDto>>;
