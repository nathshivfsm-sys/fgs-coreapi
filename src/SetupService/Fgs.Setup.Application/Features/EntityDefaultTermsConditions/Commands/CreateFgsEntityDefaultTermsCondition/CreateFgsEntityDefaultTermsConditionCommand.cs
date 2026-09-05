using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.CreateFgsEntityDefaultTermsCondition;

public sealed record CreateFgsEntityDefaultTermsConditionCommand(FgsEntityDefaultTermsConditionCreateDto Dto)
    : IRequest<ApiResponse<FgsEntityDefaultTermsConditionDetailDto>>;
