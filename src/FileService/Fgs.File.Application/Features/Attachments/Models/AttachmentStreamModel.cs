using Fgs.File.Application.Features.Attachments;

namespace Fgs.File.Application.Features.Attachments.Models;

public sealed class AttachmentStreamModel : IAsyncDisposable
{
    public required Stream Content { get; init; }

    public required string ContentType { get; init; }

    public required long ContentLength { get; init; }

    public string? ETag { get; init; }

    public DateTimeOffset? LastModified { get; init; }

    public required string FileDownloadName { get; init; }

    public async ValueTask DisposeAsync() => await Content.DisposeAsync();
}
