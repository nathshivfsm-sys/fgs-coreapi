using Fgs.Platform.Application.Reporting;

namespace Fgs.Platform.Infrastructure.Reporting;

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
