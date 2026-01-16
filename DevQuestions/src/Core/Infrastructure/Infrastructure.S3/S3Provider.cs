// DevQuestions.Infrastructure.S3

using Shared.FilesStorage;

namespace Infrastructure.S3;

public class S3Provider : IFilesProvider
{
    public async Task<string> UploadAsync(Stream stream, string key, string bucket)
    {
        await Task.Delay(500);
        throw new NotImplementedException();
    }

    public async Task<string> GetUrlByIdAsync(string fileId, CancellationToken cancellationToken)
    {
        await Task.Delay(500);
        throw new NotImplementedException();
    }

    public async Task<Dictionary<Guid, string>> GetUrlsByIdsAsync(IEnumerable<Guid> fileIds, CancellationToken cancellationToken)
    {
        await Task.Delay(500);
        throw new NotImplementedException();
    }
}