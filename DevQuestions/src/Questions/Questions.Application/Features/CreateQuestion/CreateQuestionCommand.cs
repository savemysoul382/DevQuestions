// DevQuestions.Application

using CSharpFunctionalExtensions;
using Questions.Contracts.Dtos;
using Shared;
using Shared.Abstractions;

namespace Questions.Application.Features.CreateQuestion;

public record CreateQuestionCommand(CreateQuestionDto QuestionDto) : ICommand<Guid>;