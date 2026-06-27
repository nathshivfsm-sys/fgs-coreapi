using Fgs.File.Domain.Entities;

namespace Fgs.File.Application.Common;

public static class FileUploadState
{
    public static bool IsPending(FgsFile file) => file.FileSizeBytes == 0;

    public static bool IsCompleted(FgsFile file) => file.FileSizeBytes > 0;
}
