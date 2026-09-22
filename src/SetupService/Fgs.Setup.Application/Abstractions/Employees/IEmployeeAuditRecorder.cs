using Fgs.Contracts.Audit;

namespace Fgs.Setup.Application.Abstractions.Employees;

public interface IEmployeeAuditRecorder
{
    Task RecordAsync(RecordAuditEventRequest request, CancellationToken cancellationToken = default);
}
