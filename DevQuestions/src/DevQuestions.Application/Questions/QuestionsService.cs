using DevQuestions.Application.Extensions;
using DevQuestions.Application.FullTextSearch;
using DevQuestions.Application.Questions.Fails.Exceptions;
using DevQuestions.Contracts.Questions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Shared;

namespace DevQuestions.Application.Questions;

public class QuestionsService : IQuestionsService
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<QuestionsService> _logger;
    private readonly IValidator<CreateQuestionDto> _validator;
    private readonly ISearchProvider _searchProvider;

    public QuestionsService(
        IQuestionsRepository questionsRepository,
        IValidator<CreateQuestionDto> validator,
        ISearchProvider searchProvider,
        ILogger<QuestionsService> logger)
    {
        _questionsRepository = questionsRepository;
        _validator = validator;
        _searchProvider = searchProvider;
        _logger = logger;
    }

    public async Task<Guid> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken)
    {
        // проверка валидности
        ValidationResult validationResult = await _validator.ValidateAsync(instance: questionDto, cancellation: cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToErrors();

            throw new QuestionValidationException(errors: errors);
        }

        // валидация бизнес логики
        int openUserQuestionsCount = await _questionsRepository
            .GetOpenUserQuestionsASync(userId: questionDto.UserId, cancellationToken: CancellationToken.None);

        if (openUserQuestionsCount >= 3)
        {
            throw new ToManyQuestionsException();
        }

        var existedQuestion = await _questionsRepository.GetByIdAsync(questionId: Guid.Empty, cancellationToken: cancellationToken);

        // создание сущности Question
        var questionId = Guid.NewGuid();

        var question = new Question(
            id: questionId,
            title: questionDto.Title,
            text: questionDto.Text,
            userId: questionDto.UserId,
            null,
            questionDto.TagIds.ToList());

        // сохранение в БД
        await _questionsRepository.AddAsync(question: question, cancellationToken: CancellationToken.None);

        // логирование об успешном или неуспешном сохранении
        _logger.LogInformation("Question with ID {QuestionId} created successfully.", questionId);

        await _searchProvider.IndexQuestionAsync(question: question);
        return questionId;
    }

    /*
    public async Task Update(
        Guid questionId,
        UpdateQuestionDto request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(500);
    }

    public async Task Delete(Guid questionId, CancellationToken cancellationToken)
    {
        await Task.Delay(500);
    }

    public async Task SelectSolution(
        Guid questionId,
        Guid answerId,
        CancellationToken cancellationToken)
    {
        await Task.Delay(500);
    }

    public async Task AddAnswer(
        Guid questionId,
        AddAnswerDto request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(500);
    }
    */
}