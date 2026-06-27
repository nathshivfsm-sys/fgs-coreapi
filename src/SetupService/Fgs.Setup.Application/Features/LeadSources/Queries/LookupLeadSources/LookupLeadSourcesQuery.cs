using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Queries.LookupLeadSources;

public sealed record LookupLeadSourcesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<LeadSourceLookupDto>>>;
