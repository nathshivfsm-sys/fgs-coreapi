using Fgs.File.Application.Abstractions.Storage;
using Microsoft.Extensions.Caching.Memory;

namespace Fgs.File.Infrastructure.Storage;

public sealed class FileUploadSessionStore(IMemoryCache memoryCache) : IFileUploadSessionStore
{
    private static string CacheKey(Guid uploadId) => $"file-upload:{uploadId:N}";

    public void Save(FileUploadSession session) =>
        memoryCache.Set(
            CacheKey(session.UploadId),
            session,
            new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = session.ExpiresAt
            });

    public FileUploadSession? Get(Guid uploadId) =>
        memoryCache.TryGetValue(CacheKey(uploadId), out FileUploadSession? session)
            ? session
            : null;

    public void Remove(Guid uploadId) => memoryCache.Remove(CacheKey(uploadId));
}
