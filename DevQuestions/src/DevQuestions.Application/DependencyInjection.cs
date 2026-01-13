using DevQuestions.Application.Abstractions;
using DevQuestions.Application.Questions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddScoped<IQuestionsService, QuestionsService>();

            // services.AddScoped<ICommandHandler<Guid, CreateQuestionCommand>, CreateQuestionHandler>();
            // services.AddScoped<ICommandHandler<Guid, AddAnswerCommand>, AddAnswerHandler>();
            var assembly = typeof(DependencyInjection).Assembly;

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}