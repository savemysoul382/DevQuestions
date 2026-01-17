using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions;

namespace Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedDependencies(this IServiceCollection services)
        {
            // В модульном монолите реализацию валидации и командхендлеры каждый модуль должен делать у себя.
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            // services.AddScoped<IQuestionsService, QuestionsService>();
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

            return services;
        }
    }
}