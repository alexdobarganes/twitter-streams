using Challenge.Consumer.API.Events;
using Challenge.Consumer.API.Infrastructure;

namespace Challenge.Consumer.API;
public static class Extensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName is not null)
            .ToArray();

        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection AddDispatchers(this IServiceCollection services)
    {
        return services.AddSingleton<IEventDispatcher, InMemoryEventDispatcher>();
    }

    public static IServiceCollection AddTwitterStreams(this IServiceCollection services, IConfiguration configuration) {
        services
            .Configure<TwitterApiOptions>(configuration.GetSection(TwitterApiOptions.SectionName))
            .AddSingleton<TwitterStreamClient>()
            .AddSingleton<IEventStream, TwitterStreamService>();
        return services;
    }

    public static IEventStream StreamEvents(this IApplicationBuilder app)
    {
        return app.ApplicationServices.GetRequiredService<IEventStream>();
    }
}
