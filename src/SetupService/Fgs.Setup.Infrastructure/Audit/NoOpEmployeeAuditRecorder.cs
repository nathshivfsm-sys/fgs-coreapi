using Fgs.Contracts.Audit;
using Fgs.Setup.Application.Abstractions.Employees;

namespace Fgs.Setup.Infrastructure.Audit;

public sealed class NoOpEmployeeAuditRecorder : IEmployeeAuditRecorder
{
    public Task RecordAsync(RecordAuditEventRequest request, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
