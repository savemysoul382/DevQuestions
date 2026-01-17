// DevQuestions.Application

using Questions.Contracts.Dtos;
using Shared.Abstractions;

namespace Questions.Application.Features.CreateQuestion;

public record CreateQuestionCommand(CreateQuestionDto QuestionDto) : IValidation, ILogging;