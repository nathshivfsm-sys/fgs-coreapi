using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Queries.LookupFgsEntityDefaultTermsConditions;

public sealed record LookupFgsEntityDefaultTermsConditionsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsEntityDefaultTermsConditionLookupDto>>>;
