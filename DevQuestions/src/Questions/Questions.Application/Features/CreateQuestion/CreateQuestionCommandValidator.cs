// Questions.Application

using FluentValidation;

namespace Questions.Application.Features.CreateQuestion;

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionDto.Title)
            .NotEmpty().WithMessage("Заголовок не может быть пустым.")
            .MaximumLength(500).WithMessage("Заголовок невалидный.");

        RuleFor(x => x.QuestionDto.Text)
            .NotEmpty().WithMessage("Текст не может быть пустым.")
            .MaximumLength(5000).WithMessage("Текст невалидный.");

        RuleFor(x => x.QuestionDto.UserId).NotEmpty();
    }
}