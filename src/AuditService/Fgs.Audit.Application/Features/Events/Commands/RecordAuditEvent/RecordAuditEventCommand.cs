using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Contracts.Api;
using Fgs.Contracts.Audit;
using MediatR;

namespace Fgs.Audit.Application.Features.Events.Commands.RecordAuditEvent;

public sealed record RecordAuditEventCommand(RecordAuditEventRequest Request)
    : IRequest<ApiResponse<AuditEventDetailDto>>;
