// DevQuestions.Application

namespace DevQuestions.Application.Tags;

public interface ITagsRepository
{
    Task<IReadOnlyList<string>> GetTagsAsync(IEnumerable<Guid> tagsIds, CancellationToken cancellationToken);
}