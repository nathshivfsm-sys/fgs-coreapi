using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Queries.GetLeadStatusById;

public sealed record GetLeadStatusByIdQuery(long Id)
    : IRequest<ApiResponse<LeadStatusDetailDto>>;
