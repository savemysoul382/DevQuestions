using CSharpFunctionalExtensions;
using Questions.Domain;
using Shared;
using Shared.FullTextSearch;

namespace Infrastructure.ElasticSearch;

public class ElasticSearchProvider : ISearchProvider
{
    public async Task<List<Guid>> SearchAsync(string query)
    {
        await Task.Delay(500);
        throw new NotImplementedException();
    }

    public async Task<UnitResult<Failure>> IndexQuestionAsync(Question question)
    {
        await Task.Delay(500);
        try
        {
            // _elastic.Search();
        }
        catch (Exception e)
        {
            return Error.Failure("search.error", message: e.Message).ToFailure();
        }

        return UnitResult.Success<Failure>();
    }
}