// DevQuestions.Application

using Shared;

namespace DevQuestions.Application.Questions.Fails;

public partial class Errors
{
    public static class Question
    {
        public static Error ToManyQuestions => Error.Failure(
            code: "Question.TooManyQuestions",
            message: "There are too many questions requested.");

        public static Error QuestionNotFound => Error.NotFound(
            message: "The question was not found.");

        public static Error QuestionTitleIsRequired => Error.Validation(
            code: "Question.Title.IsRequired",
            message: "The question title is required.");

        public static Error QuestionTitleTooLong => Error.Failure(
            code: "Question.Title.TooLong",
            message: "The question title is too long.");

        public static Error QuestionContentIsRequired => Error.Validation(
            code: "Question.Content.IsRequired",
            message: "The question content is required.");
    }
}