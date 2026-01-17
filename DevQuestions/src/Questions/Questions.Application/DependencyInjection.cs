using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Questions.Application.Decorators;
using Shared.Abstractions;

namespace Questions.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddScoped<IQuestionsService, QuestionsService>();

            // services.AddScoped<ICommandHandler<Guid, CreateQuestionCommand>, CreateQuestionCommandHandler>();
            // services.AddScoped<ICommandHandler<Guid, AddAnswerCommand>, AddAnswerCommandHandler>();
            var assembly = typeof(DependencyInjection).Assembly;

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            // services.AddScoped<I, TestDecorator>(s =>
            // {
            //    var a = s.GetRequiredService<A>();
            //    return new TestDecorator(a);
            // });

            // services.AddScoped<I, A>();
            // services.Decorate<I, TestDecorator>();
            // вызов идёт в обратном порядке, сначала логинг, потом валидация
            services.TryDecorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator<,>));
            services.TryDecorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator<,>));
            return services;
        }
    }
}