using Fgs.Contracts.Clients;
using Fgs.Contracts.CredentialAudit;
using Fgs.Setup.Application.Abstractions.Credentials;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Infrastructure.Audit;

public sealed class RefitCredentialAuditRecorder(
    IAuditClient auditClient,
    ILogger<RefitCredentialAuditRecorder> logger) : ICredentialAuditRecorder
{
    public async Task RecordAsync(RecordCredentialAuditRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await auditClient.RecordCredentialAuditAsync(request, cancellationToken);
            if (!response.Success)
            {
                logger.LogWarning(
                    "Credential audit write failed (ActionType={ActionType}, CredentialId={CredentialId}): {Errors}",
                    request.ActionType,
                    request.CredentialId,
                    string.Join("; ", response.Errors));
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                ex,
                "Credential audit write failed (ActionType={ActionType}, CredentialId={CredentialId}).",
                request.ActionType,
                request.CredentialId);
        }
    }
}
