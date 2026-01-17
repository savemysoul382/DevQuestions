// DevQuestions.Application

using Microsoft.EntityFrameworkCore;
using Questions.Contracts.Dtos;
using Questions.Contracts.Responses;
using Questions.Domain;
using Shared.Abstractions;

namespace Questions.Application.Features.GetQuestionsWithFilters;

public class GetQuestionsWithFilters : IQueryHandler<QuestionResponse, GetQuestionsWithFiltersQuery>
{
    // private readonly IFilesProvider _filesProvider;
    // private readonly ITagsContract _tagsContract;
    // private readonly ISqlConnectionFactory _connectionFactory;
    private readonly IQuestionsReadDbContext _questionsDbContext;

    // IFilesProvider filesProvider,
    // ITagsContract tagsContract,
    // ISqlConnectionFactory connectionFactory
    public GetQuestionsWithFilters(
        IQuestionsReadDbContext questionsDbContext)
    {
        // this._filesProvider = filesProvider;  //this._tagsContract = tagsContract; this._connectionFactory = connectionFactory;
        this._questionsDbContext = questionsDbContext;
    }

    public async Task<QuestionResponse> Handle(GetQuestionsWithFiltersQuery query, CancellationToken cancellationToken)
    {
        // Dapper approach example
        // var connection = _connectionFactory.Create();
        // await connection.ExecuteReaderAsync("SELECT * FROM Questions");
        var questions = await this._questionsDbContext.ReadQuestions
            .Include(q => q.Solution)
            .Skip(query.GetQuestionsDto.Page * query.GetQuestionsDto.PageSize)
            .Take(count: query.GetQuestionsDto.PageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        var count = await this._questionsDbContext.ReadQuestions.LongCountAsync(cancellationToken: cancellationToken);
        var screenshotIds = questions.Where(q => q.ScreenshotId != null).Select(q => q.ScreenshotId!.Value).ToList();

        // var filesDict = await _filesProvider.GetUrlsByIdsAsync(fileIds: screenshotIds, cancellationToken: cancellationToken);
        var questionTags = questions.SelectMany(q => q.Tags);

        // var tags = await this._tagsContract.GetByIds(new GetByIdsDto(questionTags.ToArray()));
        var questionDtos = questions.Select(q => new QuestionDto(
            Id: q.Id,
            Title: q.Title,
            Text: q.Text,
            UserId: q.UserId,
            ScreenshotUrl: "из строки справа", // ScreenshotUrl: q.ScreenshotId != null ? filesDict[key: q.ScreenshotId.Value] : null,
            SolutionId: q.Solution?.Id,
            Tags: [], // tags.Select(t => t.Name),
            q.Status.ToRussianString())).ToList();

        return new QuestionResponse(Questions: questionDtos, TotalCount: count);
    }
}