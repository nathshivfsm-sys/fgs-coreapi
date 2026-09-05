using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TermsConditions.Commands.CreateFgsTermsCondition;

public sealed record CreateFgsTermsConditionCommand(FgsTermsConditionCreateDto Dto)
    : IRequest<ApiResponse<FgsTermsConditionDetailDto>>;
