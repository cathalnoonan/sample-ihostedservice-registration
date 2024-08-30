namespace Cathal.IHostedServiceRegistration;

public static class RegistrationExtensions
{
    /// <summary>
    /// Registers the hosted service with the dependency injection container using its own type as the key.
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="THostedService"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddHostedServiceAsSelf<THostedService>(this IServiceCollection services)
        where THostedService : class, IHostedService
    {
        services.AddSingleton<THostedService>();
        services.AddHostedService<THostedService>(sp => sp.GetRequiredService<THostedService>());

        return services;
    }

    /// <summary>
    /// Registers the hosted service with the dependency injection container using its own type as the key.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="implementationFactory"></param>
    /// <typeparam name="THostedService"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddHostedServiceAsSelf<THostedService>(this IServiceCollection services, Func<IServiceProvider, THostedService> implementationFactory)
        where THostedService : class, IHostedService
    {
        services.AddSingleton<THostedService>(implementationFactory);
        services.AddHostedService<THostedService>(sp => sp.GetRequiredService<THostedService>());

        return services;
    }
}
