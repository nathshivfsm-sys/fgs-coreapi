using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Queries.GetFgsEntityDefaultTermsConditionById;

public sealed record GetFgsEntityDefaultTermsConditionByIdQuery(long Id)
    : IRequest<ApiResponse<FgsEntityDefaultTermsConditionDetailDto>>;
