// DevQuestions.Application

using Questions.Contracts.Dtos;
using Shared.Abstractions;

namespace Questions.Application.Features.GetQuestionsWithFilters;

public record GetQuestionsWithFiltersQuery(GetQuestionsDto GetQuestionsDto) : IQuery;