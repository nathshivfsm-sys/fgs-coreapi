using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.LookupLeadDisqualificationReasons;

public sealed record LookupLeadDisqualificationReasonsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<LeadDisqualificationReasonLookupDto>>>;
