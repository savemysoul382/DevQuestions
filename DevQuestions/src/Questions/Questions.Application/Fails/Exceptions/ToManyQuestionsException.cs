// DevQuestions.Application

using Shared.Exceptions;

namespace Questions.Application.Fails.Exceptions;

public class ToManyQuestionsException : NotFoundException
{
    public ToManyQuestionsException()
        : base([Errors.Question.ToManyQuestions])
    {
    }
}