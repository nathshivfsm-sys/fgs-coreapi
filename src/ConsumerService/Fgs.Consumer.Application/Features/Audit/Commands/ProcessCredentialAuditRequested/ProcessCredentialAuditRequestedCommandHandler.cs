using Fgs.Contracts.Clients;
using Fgs.Contracts.CredentialAudit;
using MediatR;

namespace Fgs.Consumer.Application.Features.Audit.Commands.ProcessCredentialAuditRequested;

public sealed class ProcessCredentialAuditRequestedCommandHandler(IAuditClient auditClient)
    : IRequestHandler<ProcessCredentialAuditRequestedCommand>
{
    public async Task Handle(
        ProcessCredentialAuditRequestedCommand request,
        CancellationToken cancellationToken)
    {
        var evt = request.Event;
        var auditRequest = new RecordCredentialAuditRequest(
            evt.TenantId,
            evt.CompanyId,
            evt.CredentialId,
            evt.ActionType,
            evt.Remarks,
            evt.OldVersionNo,
            evt.NewVersionNo,
            evt.CreatedBy);

        var response = await auditClient.RecordCredentialAuditAsync(auditRequest, cancellationToken);
        if (!response.Success)
        {
            var message = response.Errors.Count > 0
                ? string.Join("; ", response.Errors)
                : "Credential audit write failed.";
            throw new InvalidOperationException(message);
        }
    }
}
