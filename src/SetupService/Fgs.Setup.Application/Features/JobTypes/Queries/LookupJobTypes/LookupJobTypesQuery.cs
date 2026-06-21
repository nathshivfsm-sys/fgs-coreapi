using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Queries.LookupJobTypes;

public sealed record LookupJobTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<JobTypeLookupDto>>>;
