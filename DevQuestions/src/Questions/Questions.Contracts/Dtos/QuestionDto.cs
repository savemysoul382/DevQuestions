// DevQuestions.Application

namespace Questions.Contracts.Dtos;

/// <summary>
/// Question Response DTO
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Text"></param>
/// <param name="UserId"></param>
/// <param name="ScreenshotUrl"></param>
/// <param name="SolutionId"></param>
/// <param name="Tags"></param>
/// <param name="Status"></param>
public record QuestionDto(
    Guid Id,
    string Title,
    string Text,
    Guid UserId,
    string? ScreenshotUrl,
    Guid? SolutionId,
    IEnumerable<string> Tags,
    string Status);