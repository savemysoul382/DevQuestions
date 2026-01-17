// DevQuestions.Application

using Shared;

namespace Questions.Application.Fails;

public partial class Errors
{
    public static class General
    {
        public static Error NotFound(Guid id) =>
            Error.Failure(code: "record.not.found", message: $"Запись по id - {id} не найдена.");
    }

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

    public static Failure NotEnoughRating()
    {
        var error = Error.Failure(
            code: "not.enough.rating",
            message: "Недостаточно рейтинга.");

        return error;
    }
}