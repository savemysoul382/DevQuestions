// DevQuestions.Infrastructure.S3

using DevQuestions.Application.FilesStorage;

namespace DevQuestions.Infrastructure.S3;

public class S3Provider : IFilesProvider
{
    public async Task<string> UploadAsync(Stream stream, string key, string bucket)
    {
        await Task.Delay(500);
        throw new NotImplementedException();
    }
}