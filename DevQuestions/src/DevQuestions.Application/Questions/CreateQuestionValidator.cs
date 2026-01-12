using DevQuestions.Contracts.Questions;
using FluentValidation;

namespace DevQuestions.Application.Questions
{
    public class CreateQuestionValidator : AbstractValidator<CreateQuestionDto>
    {
        public CreateQuestionValidator()
        {
            RuleFor(x => x.Title).NotNull().MaximumLength(500).WithMessage("Заголовок невалидный");

            RuleFor(x => x.Text).NotNull().MaximumLength(5000).WithMessage("Текст невалидный");

            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}