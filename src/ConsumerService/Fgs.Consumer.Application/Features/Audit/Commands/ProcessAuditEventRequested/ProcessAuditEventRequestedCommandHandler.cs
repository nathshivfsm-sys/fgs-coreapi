using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.Consumer.Application.Features.Audit.Commands.ProcessAuditEventRequested;

public sealed class ProcessAuditEventRequestedCommandHandler(IAuditClient auditClient)
    : IRequestHandler<ProcessAuditEventRequestedCommand>
{
    public async Task Handle(
        ProcessAuditEventRequestedCommand request,
        CancellationToken cancellationToken)
    {
        var response = await auditClient.RecordAuditEventAsync(request.Event.Request, cancellationToken);
        if (!response.Success)
        {
            var message = response.Errors.Count > 0
                ? string.Join("; ", response.Errors)
                : "Audit event write failed.";
            throw new InvalidOperationException(message);
        }
    }
}
