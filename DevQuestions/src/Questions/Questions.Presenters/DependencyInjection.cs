// Questions.Presenters

using Microsoft.Extensions.DependencyInjection;
using Questions.Application;

namespace Questions.Presenters;

public static class DependencyInjection
{
    public static IServiceCollection AddQuestionsModule(this IServiceCollection services)
    {
        // регистрируем остальные зависимости из соседних модулей Question
        services.AddApplication();
        return services;
    }
}