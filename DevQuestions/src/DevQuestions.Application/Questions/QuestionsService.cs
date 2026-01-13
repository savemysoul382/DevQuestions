using CSharpFunctionalExtensions;
using DevQuestions.Application.Communication;
using DevQuestions.Application.Database;
using DevQuestions.Application.Extensions;
using DevQuestions.Application.Questions.Fails;
using DevQuestions.Contracts.Questions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Shared;
using Error = Shared.Error;

namespace DevQuestions.Application.Questions;

/// <summary>
/// Остался для примера.
/// </summary>
public class QuestionsService : IQuestionsService
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<QuestionsService> _logger;
    private readonly IValidator<CreateQuestionDto> _createQuestionDtoValidator;
    private readonly IValidator<AddAnswerDto> _addAnswerDtoValidator;

    // private readonly IUsersCommunicationService _usersService;
    public QuestionsService(
        IQuestionsRepository questionsRepository,
        IValidator<CreateQuestionDto> createQuestionDtoValidator,
        ILogger<QuestionsService> logger,
        IValidator<AddAnswerDto> addAnswerDtoValidator) // IUsersCommunicationService usersService
    {
        _questionsRepository = questionsRepository;
        _createQuestionDtoValidator = createQuestionDtoValidator;
        _addAnswerDtoValidator = addAnswerDtoValidator;

        // _usersService = usersService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<Guid, Failure>> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken)
    {
        // проверка валидности
        ValidationResult validationResult = await _createQuestionDtoValidator.ValidateAsync(instance: questionDto, cancellation: cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }

        var calculator = new QuestionCalculator();

        Result<int, Failure> calculateResult = calculator.Calculate();

        if (calculateResult.IsFailure)
        {
            return calculateResult.Error;
        }

        int calculateResultValue = calculateResult.Value;

        // валидация бизнес логики
        int openUserQuestionsCount = await _questionsRepository
            .GetOpenUserQuestionsAsync(userId: questionDto.UserId, cancellationToken: CancellationToken.None);

        if (openUserQuestionsCount >= 3)
        {
            return Errors.Question.ToManyQuestions.ToFailure();
        }

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

        return questionId;
    }

    public async Task<Result<Guid, Failure>> AddAnswer(
        Guid questionId,
        AddAnswerDto addAnswerDto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _addAnswerDtoValidator.ValidateAsync(instance: addAnswerDto, cancellation: cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }

        // var userRatingResult = await _usersService.GetUserRatingAsync(userId: addAnswerDto.UserId, cancellationToken: cancellationToken);
        // if (userRatingResult.IsFailure)
        // {
        //    return userRatingResult.Error;
        // }

        // if (userRatingResult.Value <= 0)
        // {
        //    _logger.LogError("User with ID {UserId} has has no rating.", addAnswerDto.UserId);
        //    return Errors.NotEnoughRating();
        // }

        // var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken: cancellationToken);
        (_, bool isFailure, Question? question, Failure? error) = await _questionsRepository.GetByIdAsync(questionId: questionId, cancellationToken: cancellationToken);

        if (isFailure)
        {
            return error;
        }

        var answer = new Answer(
            id: Guid.NewGuid(),
            userId: addAnswerDto.UserId,
            text: addAnswerDto.Text,
            questionId: questionId);

        question?.Answers.Add(item: answer);

        Guid addAnswer = await _questionsRepository.SaveAsync(question!, cancellationToken: cancellationToken);

        // transaction.Commit();
        _logger.LogInformation("Answer with ID {answerId} added to question {QuestionId} successfully.", answer.Id, questionId);

        return answer.Id;
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
    */

    public class QuestionCalculator
    {
        public Result<int, Failure> Calculate()
        {
            return Error.Failure(code: string.Empty, message: string.Empty).ToFailure();
        }
    }
}