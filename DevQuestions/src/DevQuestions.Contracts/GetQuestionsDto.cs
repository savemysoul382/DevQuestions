// DevQuestions.Contracts

namespace DevQuestions.Contracts;

public record GetQuestionsDto(string Search, Guid[] TagIds, int Page, int PageSize);