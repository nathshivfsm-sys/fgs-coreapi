using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TermsConditions.Queries.LookupFgsTermsConditions;

public sealed record LookupFgsTermsConditionsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsTermsConditionLookupDto>>>;
