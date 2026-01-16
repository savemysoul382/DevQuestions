// DevQuestions.Application

namespace Shared.FilesStorage;

public interface IFilesProvider
{
    Task<string> UploadAsync(Stream stream, string key, string bucket);

    Task<string> GetUrlByIdAsync(string fileId, CancellationToken cancellationToken);

    Task<Dictionary<Guid, string>> GetUrlsByIdsAsync(IEnumerable<Guid> fileIds, CancellationToken cancellationToken);
}