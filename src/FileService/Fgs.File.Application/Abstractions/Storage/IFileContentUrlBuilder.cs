namespace Fgs.File.Application.Abstractions.Storage;

public interface IFileContentUrlBuilder
{
    string BuildContentUrl(long fileId);
}
