using Fgs.Notification.Application.Reporting;

namespace Fgs.Notification.Infrastructure.Reporting;

public sealed class PlaceholderReportExporter : IReportExporter
{
    public Task<ReportExportResult> ExportAsync(
        ReportExportRequest request,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(new ReportExportResult(
            false,
            null,
            "Reporting module is scaffolded only; export is not implemented."));
}
