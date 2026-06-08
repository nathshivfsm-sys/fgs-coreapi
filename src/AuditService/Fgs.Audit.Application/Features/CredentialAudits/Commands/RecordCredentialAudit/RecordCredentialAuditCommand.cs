using Fgs.Contracts.Api;
using Fgs.Contracts.CredentialAudit;
using MediatR;

namespace Fgs.Audit.Application.Features.CredentialAudits.Commands.RecordCredentialAudit;

public sealed record RecordCredentialAuditCommand(RecordCredentialAuditRequest Request)
    : IRequest<ApiResponse<object>>;
