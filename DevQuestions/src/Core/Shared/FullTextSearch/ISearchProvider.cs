using CSharpFunctionalExtensions;

namespace Shared.FullTextSearch;

public interface ISearchProvider
{
    Task<List<Guid>> SearchAsync(string query);

    Task<UnitResult<Failure>> IndexQuestionAsync(Question question);
}