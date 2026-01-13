// DevQuestions.Application

using CSharpFunctionalExtensions;
using DevQuestions.Contracts.Questions;
using Shared;

namespace DevQuestions.Application.Questions;

public interface IQuestionsService
{
    /// <summary>
    /// Создание вопроса.
    /// </summary>
    /// <param name="questionDto">DTO для создания вопроса.</param>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>Результат работы метода. Либо ID созданного вопроса, либо список ошибок.</returns>
    Task<Result<Guid, Failure>> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление ответа к вопросу.
    /// </summary>
    /// <param name="questionId">ID вопроса.</param>
    /// <param name="addAnswerDto">DTO для добавления ответа на вопрос.</param>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>Результат работы метода. Либо ID добавленного ответа, либо список ошибок.</returns>
    Task<Result<Guid, Failure>> AddAnswer(Guid questionId, AddAnswerDto addAnswerDto, CancellationToken cancellationToken);
}