// DevQuestions.Application

using DevQuestions.Application.Exceptions;

namespace DevQuestions.Application.Questions.Fails.Exceptions;

public class ToManyQuestionsException : NotFoundException
{
    public ToManyQuestionsException()
        : base([Errors.Question.ToManyQuestions])
    {
    }
}