using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.UpdateFgsEntityDefaultTermsCondition;

public sealed record UpdateFgsEntityDefaultTermsConditionCommand(long Id, FgsEntityDefaultTermsConditionUpdateDto Dto)
    : IRequest<ApiResponse<FgsEntityDefaultTermsConditionDetailDto>>;
