// DevQuestions.Application

using CSharpFunctionalExtensions;
using DevQuestions.Application.Abstractions;
using DevQuestions.Application.FilesStorage;
using DevQuestions.Application.Tags;
using DevQuestions.Contracts.Questions.Dtos;
using DevQuestions.Contracts.Questions.Responses;
using DevQuestions.Domain.Questions;
using Shared;

namespace DevQuestions.Application.Questions.Features.GetQuestionsWithFilters;

public class GetQuestionsWithFilters : IHandler<QuestionResponse, GetQuestionsWithFiltersCommand>
{
    private readonly IFilesProvider _filesProvider;
    private readonly ITagsRepository _tagsRepository;
    private readonly IQuestionsReadDbContext _questionsDbContext;

    public GetQuestionsWithFilters(
        IQuestionsRepository questionsRepository,
        IFilesProvider filesProvider,
        ITagsRepository tagsRepository,
        IQuestionsReadDbContext questionsDbContext)
    {
        this._filesProvider = filesProvider;
        this._tagsRepository = tagsRepository;
        this._questionsDbContext = questionsDbContext;
    }

    public async Task<Result<QuestionResponse, Failure>> HandleAsync(GetQuestionsWithFiltersCommand command, CancellationToken cancellationToken)
    {
        var questions = await this._questionsDbContext.ReadQuestions
            .

        (IReadOnlyList<Question> questions, long count) = await this._questionsRepository.GetQuestionWithFiltersAsync(command: command, cancellationToken: cancellationToken);

        var screenshotIds = questions.Where(q => q.ScreenshotId != null).Select(q => q.ScreenshotId!.Value).ToList();
        var filesDict = await _filesProvider.GetUrlsByIdsAsync(fileIds: screenshotIds, cancellationToken: cancellationToken);

        var questionTags = questions.SelectMany(q => q.Tags);
        var tags = await this._tagsRepository.GetTagsAsync(tagsIds: questionTags, cancellationToken: cancellationToken);

        var questionDtos = questions.Select(q => new QuestionDto(
            Id: q.Id,
            Title: q.Title,
            Text: q.Text,
            UserId: q.UserId,
            ScreenshotUrl: q.ScreenshotId != null ? filesDict[key: q.ScreenshotId.Value] : null,
            SolutionId: q.Solution?.Id,
            Tags: tags,
            q.Status.ToRussianString())).ToList();

        return new QuestionResponse(Questions: questionDtos, count);
    }
}