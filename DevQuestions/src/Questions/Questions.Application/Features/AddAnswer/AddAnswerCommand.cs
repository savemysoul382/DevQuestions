// DevQuestions.Application

using Questions.Contracts.Dtos;
using Shared.Abstractions;

namespace Questions.Application.Features.AddAnswer;

public record AddAnswerCommand(Guid QuestionId, AddAnswerDto AddAnswerDto) : ICommand;