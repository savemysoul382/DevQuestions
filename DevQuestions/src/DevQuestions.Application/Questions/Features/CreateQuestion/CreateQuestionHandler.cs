// DevQuestions.Application

using CSharpFunctionalExtensions;
using DevQuestions.Application.Abstractions;
using DevQuestions.Application.Extensions;
using DevQuestions.Application.Questions.Fails;
using DevQuestions.Contracts.Questions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Shared;

namespace DevQuestions.Application.Questions.CreateQuestion;

public class CreateQuestionHandler : ICommandHandler<Guid, CreateQuestionCommand>
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly IValidator<CreateQuestionDto> _createQuestionDtoValidator;
    private readonly ILogger<QuestionsService> _logger;

    public CreateQuestionHandler(
        IQuestionsRepository questionsRepository,
        IValidator<CreateQuestionDto> createQuestionDtoValidator,
        ILogger<QuestionsService> logger)
    {
        _questionsRepository = questionsRepository;
        _logger = logger;
        _createQuestionDtoValidator = createQuestionDtoValidator;
    }

    public async Task<Result<Guid, Failure>> HandleAsync(CreateQuestionCommand command, CancellationToken cancellationToken)
    {
        // проверка валидности
        ValidationResult validationResult = await _createQuestionDtoValidator.ValidateAsync(instance: command.QuestionDto, cancellation: cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }

        var calculator = new QuestionsService.QuestionCalculator();

        Result<int, Failure> calculateResult = calculator.Calculate();

        if (calculateResult.IsFailure)
        {
            return calculateResult.Error;
        }

        int calculateResultValue = calculateResult.Value;

        // валидация бизнес логики
        int openUserQuestionsCount = await _questionsRepository
            .GetOpenUserQuestionsAsync(userId: command.QuestionDto.UserId, cancellationToken: CancellationToken.None);

        if (openUserQuestionsCount >= 3)
        {
            return Errors.Question.ToManyQuestions.ToFailure();
        }

        // создание сущности Question
        var questionId = Guid.NewGuid();

        var question = new Question(
            id: questionId,
            title: command.QuestionDto.Title,
            text: command.QuestionDto.Text,
            userId: command.QuestionDto.UserId,
            null,
            command.QuestionDto.TagIds.ToList());

        // сохранение в БД
        await _questionsRepository.AddAsync(question: question, cancellationToken: CancellationToken.None);

        // логирование об успешном или неуспешном сохранении
        _logger.LogInformation("Question with ID {QuestionId} created successfully.", questionId);

        return questionId;
    }
}