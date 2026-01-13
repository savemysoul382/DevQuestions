using CSharpFunctionalExtensions;
using DevQuestions.Application.Abstractions;
using DevQuestions.Application.Questions;
using DevQuestions.Application.Questions.AddAnswer;
using DevQuestions.Application.Questions.CreateQuestion;
using DevQuestions.Contracts.Questions;
using DevQuestions.Presenters.ResponseExtensions;
using Microsoft.AspNetCore.Mvc;
using Shared;

// ReSharper disable InconsistentNaming
namespace DevQuestions.Presenters.Questions;

[ApiController]
[Route("[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionsService _questionService;

    public QuestionsController(IQuestionsService questionService)
    {
        _questionService = questionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<Guid, CreateQuestionCommand> createQuestionHandler,
        [FromBody] CreateQuestionDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateQuestionCommand(QuestionDto: request);

        Result<Guid, Failure> questionIdResult = await createQuestionHandler.HandleAsync(command: command, cancellationToken: CancellationToken.None);
        return questionIdResult.IsFailure ? questionIdResult.Error.ToResponse() : Ok(value: questionIdResult.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetQuestionsDto request, CancellationToken cancellationToken)
    {
        await Task.Delay(500);
        return Ok();
    }

    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid questionId, CancellationToken cancellationToken)
    {
        await Task.Delay(500);
        return Ok();
    }

    [HttpPut("{questionId:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid questionId,
        [FromBody] UpdateQuestionDto request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(500);
        return Ok("Question deleted");
    }

    [HttpDelete("{questionId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid questionId, CancellationToken cancellationToken)
    {
        await Task.Delay(500);
        return Ok("Question deleted");
    }

    [HttpPut("{questionId:guid}/solution")]
    public async Task<IActionResult> SelectSolution(
        [FromRoute] Guid questionId,
        [FromQuery] Guid answerId,
        CancellationToken cancellationToken)
    {
        await Task.Delay(500);
        return Ok("Solution selected");
    }

    [HttpPost("{questionId:guid}/answers")]
    public async Task<IActionResult> AddAnswer(
        [FromServices] ICommandHandler<Guid, AddAnswerCommand> addAnswerHandler,
        [FromRoute] Guid questionId,
        [FromBody] AddAnswerDto request,
        CancellationToken cancellationToken)
    {
        var command = new AddAnswerCommand(QuestionId: questionId, AddAnswerDto: request);

        Result<Guid, Failure> handleResult = await addAnswerHandler.HandleAsync(command: command, cancellationToken: cancellationToken);
        return handleResult.IsFailure ? handleResult.Error.ToResponse() : Ok(value: handleResult.Value);
    }
}