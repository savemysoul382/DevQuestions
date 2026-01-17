using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Questions.Application.Behaviors;

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

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly: assembly);

                config.AddOpenBehavior(typeof(ValidationBehavior<,>));

                // config.AddOpenBehavior(typeof(TracingBehavior<,>)); идут по цепочке, может быть сколько угодно
            });

            // services.Scan(scan => scan.FromAssemblies(assembly)
            //    .AddClasses(classes => classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            //    .AsSelfWithInterfaces()
            //    .WithScopedLifetime());

            // services.Scan(scan => scan.FromAssemblies(assembly)
            //    .AddClasses(classes => classes.AssignableToAny(typeof(IQueryHandler<,>)))
            //    .AsSelfWithInterfaces()
            //    .WithScopedLifetime());
            return services;
        }
    }
}