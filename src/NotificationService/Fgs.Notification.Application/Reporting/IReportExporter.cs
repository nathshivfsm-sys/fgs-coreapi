namespace Fgs.Notification.Application.Reporting;

public interface IReportExporter
{
    Task<ReportExportResult> ExportAsync(ReportExportRequest request, CancellationToken cancellationToken = default);
}

public sealed record ReportExportRequest(string ReportKey, long? TenantId, string Format);

public sealed record ReportExportResult(bool Success, string? Location, string? Error);
