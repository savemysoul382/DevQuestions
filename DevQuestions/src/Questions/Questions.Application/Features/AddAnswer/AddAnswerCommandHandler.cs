// DevQuestions.Application

using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Questions.Contracts.Dtos;
using Questions.Domain;
using Shared;
using Shared.Abstractions;
using Shared.Extensions;

namespace Questions.Application.Features.AddAnswer;

public class AddAnswerCommandHandler : ICommandHandler<AddAnswerCommand, Guid>
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly IValidator<AddAnswerDto> _addAnswerDtoValidator;
    private readonly ILogger<QuestionsService> _logger;

    // private readonly ITransactionManager _transactionManager;
    // private readonly IUsersCommunicationService _usersService;
    public AddAnswerCommandHandler(

        // ITransactionManager transactionManager,
        // IUsersCommunicationService usersService,
#pragma warning disable SA1114
        IQuestionsRepository questionsRepository,
#pragma warning restore SA1114
        IValidator<AddAnswerDto> addAnswerDtoValidator,
        ILogger<QuestionsService> logger)
    {
        _questionsRepository = questionsRepository;
        _addAnswerDtoValidator = addAnswerDtoValidator;

        // _transactionManager = transactionManager;
        // _usersService = usersService;
        _logger = logger;
    }

    public async Task<Result<Guid, Failure>> Handle(
        AddAnswerCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _addAnswerDtoValidator.ValidateAsync(instance: command.AddAnswerDto, cancellation: cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }

        // var userRatingResult = await _usersService.GetUserRatingAsync(
        //    userId: command.AddAnswerDto.UserId,
        //    cancellationToken: cancellationToken);
        // if (userRatingResult.IsFailure)
        // {
        //    return userRatingResult.Error;
        // }

        // if (userRatingResult.Value <= 0)
        // {
        //    _logger.LogError("User with ID {UserId} has has no rating.", command.AddAnswerDto.UserId);
        //    return Errors.NotEnoughRating();
        // }

        // var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken: cancellationToken);
        (_, bool isFailure, Question? question, Failure? error) = await _questionsRepository.GetByIdAsync(
            questionId: command.QuestionId,
            cancellationToken: cancellationToken);

        if (isFailure)
        {
            return error;
        }

        var answer = new Answer(
            id: Guid.NewGuid(),
            userId: command.AddAnswerDto.UserId,
            text: command.AddAnswerDto.Text,
            questionId: command.QuestionId);

        question?.Answers.Add(item: answer);

        Guid addAnswer = await _questionsRepository.SaveAsync(question!, cancellationToken: cancellationToken);

        // transaction.Commit();
        _logger.LogInformation("Answer with ID {answerId} added to question {QuestionId} successfully.", answer.Id, command.QuestionId);

        return answer.Id;
    }
}