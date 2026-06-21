using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Queries.LookupLeadStatuses;

public sealed record LookupLeadStatusesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<LeadStatusLookupDto>>>;
