// DevQuestions.Application

using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Questions.Application.Fails;
using Questions.Contracts.Dtos;
using Questions.Domain;
using Shared;
using Shared.Abstractions;
using Shared.Extensions;

namespace Questions.Application.Features.CreateQuestion;

public class CreateQuestionCommandHandler : ICommandHandler<Guid, CreateQuestionCommand>
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly IValidator<CreateQuestionDto> _createQuestionDtoValidator;
    private readonly ILogger<QuestionsService> _logger;

    public CreateQuestionCommandHandler(
        IQuestionsRepository questionsRepository,
        IValidator<CreateQuestionDto> createQuestionDtoValidator,
        ILogger<QuestionsService> logger)
    {
        this._questionsRepository = questionsRepository;
        this._logger = logger;
        this._createQuestionDtoValidator = createQuestionDtoValidator;
    }

    public async Task<Result<Guid, Failure>> HandleAsync(CreateQuestionCommand command, CancellationToken cancellationToken)
    {
        // проверка валидности
        ValidationResult validationResult = await this._createQuestionDtoValidator.ValidateAsync(instance: command.QuestionDto, cancellation: cancellationToken);
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
        int openUserQuestionsCount = await this._questionsRepository
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
        await this._questionsRepository.AddAsync(question: question, cancellationToken: CancellationToken.None);

        // логирование об успешном или неуспешном сохранении
        this._logger.LogInformation("Question with ID {QuestionId} created successfully.", questionId);

        return questionId;
    }
}