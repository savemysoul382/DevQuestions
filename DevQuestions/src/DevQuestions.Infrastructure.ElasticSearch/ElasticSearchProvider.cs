using DevQuestions.Application.FullTextSearch;
using DevQuestions.Domain.Questions;

namespace DevQuestions.Infrastructure.ElasticSearch;

public class ElasticSearchProvider : ISearchProvider
{
    public async Task<List<Guid>> SearchAsync(string query)
    {
        await Task.Delay(500);
        throw new NotImplementedException();
    }

    public async Task IndexQuestionAsync(Question question)
    {
        await Task.Delay(500);
        throw new NotImplementedException();
    }
}