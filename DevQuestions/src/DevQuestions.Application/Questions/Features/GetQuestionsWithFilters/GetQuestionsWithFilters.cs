// DevQuestions.Application

using Dapper;
using DevQuestions.Application.Abstractions;
using DevQuestions.Application.Database;
using DevQuestions.Application.Tags;
using DevQuestions.Contracts.Questions.Dtos;
using DevQuestions.Contracts.Questions.Responses;
using DevQuestions.Domain.Questions;
using Microsoft.EntityFrameworkCore;

namespace DevQuestions.Application.Questions.Features.GetQuestionsWithFilters;

public class GetQuestionsWithFilters : IQueryHandler<QuestionResponse, GetQuestionsWithFiltersQuery>
{
    // private readonly IFilesProvider _filesProvider;
    private readonly ITagsReadDbContext _tagsReadDbContext;
    private readonly IQuestionsReadDbContext _questionsDbContext;
    private readonly ISqlConnectionFactory _connectionFactory;

    // IFilesProvider filesProvider,
    public GetQuestionsWithFilters(
        IQuestionsRepository questionsRepository,
        ITagsReadDbContext tagsReadDbContext,
        IQuestionsReadDbContext questionsDbContext, 
        ISqlConnectionFactory connectionFactory)
    {
        // this._filesProvider = filesProvider;
        this._tagsReadDbContext = tagsReadDbContext;
        this._questionsDbContext = questionsDbContext;
        this._connectionFactory = connectionFactory;
    }

    public async Task<QuestionResponse> HandleAsync(GetQuestionsWithFiltersQuery query, CancellationToken cancellationToken)
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
        var tags = await this._tagsReadDbContext.TagsRead
            .Where(t => questionTags.Contains(t.Id))
            .Select(t => t.Name)
            .ToListAsync(cancellationToken: cancellationToken);

        var questionDtos = questions.Select(q => new QuestionDto(
            Id: q.Id,
            Title: q.Title,
            Text: q.Text,
            UserId: q.UserId,
            ScreenshotUrl: "из строки справа", // ScreenshotUrl: q.ScreenshotId != null ? filesDict[key: q.ScreenshotId.Value] : null,
            SolutionId: q.Solution?.Id,
            Tags: tags,
            q.Status.ToRussianString())).ToList();

        return new QuestionResponse(Questions: questionDtos, TotalCount: count);
    }
}