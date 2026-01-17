using CSharpFunctionalExtensions;
using Framework.ResponseExtensions;
using Microsoft.AspNetCore.Mvc;
using Questions.Application;
using Questions.Application.Features.AddAnswer;
using Questions.Application.Features.CreateQuestion;
using Questions.Application.Features.GetQuestionsWithFilters;
using Questions.Contracts.Dtos;
using Questions.Contracts.Responses;
using Shared;
using Shared.Abstractions;

// ReSharper disable InconsistentNaming
namespace Questions.Presenters;

[ApiController]
[Route("[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionsService _questionService;

    public QuestionsController(IQuestionsService questionService)
    {
        this._questionService = questionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<Guid, CreateQuestionCommand> createQuestionCommandHandler,
        [FromBody] CreateQuestionDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateQuestionCommand(QuestionDto: request);

        Result<Guid, Failure> questionIdResult = await createQuestionCommandHandler.HandleAsync(command: command, cancellationToken: CancellationToken.None);
        return questionIdResult.IsFailure ? questionIdResult.Error.ToResponse() : Ok(value: questionIdResult.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromServices] IQueryHandler<QuestionResponse, GetQuestionsWithFiltersQuery> getQuestionsCommandHandler,
        [FromQuery] GetQuestionsDto request,
        CancellationToken cancellationToken)
    {
        var query = new GetQuestionsWithFiltersQuery(GetQuestionsDto: request);

        Result<QuestionResponse, Failure> response = await getQuestionsCommandHandler.HandleAsync(query: query, cancellationToken: cancellationToken);
        return response.IsFailure ? response.Error.ToResponse() : Ok(value: response.Value);
    }

    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid questionId,
        CancellationToken cancellationToken)
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
    public async Task<IActionResult> Delete(
        [FromRoute] Guid questionId,
        CancellationToken cancellationToken)
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
        [FromServices] ICommandHandler<Guid, AddAnswerCommand> addAnswerCommandHandler,
        [FromRoute] Guid questionId,
        [FromBody] AddAnswerDto request,
        CancellationToken cancellationToken)
    {
        var command = new AddAnswerCommand(QuestionId: questionId, AddAnswerDto: request);

        Result<Guid, Failure> handleResult = await addAnswerCommandHandler.HandleAsync(command: command, cancellationToken: cancellationToken);
        return handleResult.IsFailure ? handleResult.Error.ToResponse() : Ok(value: handleResult.Value);
    }
}