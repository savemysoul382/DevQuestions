// DevQuestions.Application

namespace DevQuestions.Application.FilesStorage;

public interface IFilesProvider
{
    Task<string> UploadAsync(Stream stream, string key, string bucket);
}