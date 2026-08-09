using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Audit.Application.Features.Events.Queries.GetAuditEventById;

public sealed record GetAuditEventByIdQuery(long Id)
    : IRequest<ApiResponse<AuditEventDetailDto>>;
