using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common.Options;
using Microsoft.Extensions.Options;

namespace Fgs.File.Infrastructure.Storage;

public sealed class FileContentUrlBuilder(IOptions<FileServiceOptions> options) : IFileContentUrlBuilder
{
    public string BuildContentUrl(long fileId)
    {
        var baseUrl = options.Value.PublicBaseUrl.TrimEnd('/');
        return $"{baseUrl}/api/v1/files/{fileId}/content";
    }
}
