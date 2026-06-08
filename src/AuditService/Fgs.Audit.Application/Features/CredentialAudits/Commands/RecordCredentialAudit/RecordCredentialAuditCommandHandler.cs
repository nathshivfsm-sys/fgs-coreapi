using Fgs.Audit.Application.Abstractions;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Audit.Application.Features.CredentialAudits.Commands.RecordCredentialAudit;

public sealed class RecordCredentialAuditCommandHandler(ICredentialAuditWriter writer)
    : IRequestHandler<RecordCredentialAuditCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        RecordCredentialAuditCommand request,
        CancellationToken cancellationToken)
    {
        await writer.WriteAsync(request.Request, cancellationToken);
        return ApiResponse<object>.Ok(new object(), ApiStatusCodes.Created);
    }
}
