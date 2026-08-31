using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TermsConditions.Queries.GetFgsTermsConditionById;

public sealed record GetFgsTermsConditionByIdQuery(long Id)
    : IRequest<ApiResponse<FgsTermsConditionDetailDto>>;
