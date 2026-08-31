namespace Cathal.IHostedServiceRegistration;

public static class RegistrationExtensions
{
    /// <param name="services"></param>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers the hosted service with the dependency injection container using its own type as the key.
        /// </summary>
        /// <typeparam name="THostedService"></typeparam>
        /// <returns></returns>
        public IServiceCollection AddHostedServiceAsSelf<THostedService>()
            where THostedService : class, IHostedService
        {
            services.AddSingleton<THostedService>();
            services.AddHostedService<THostedService>(sp => sp.GetRequiredService<THostedService>());

            return services;
        }

        /// <summary>
        /// Registers the hosted service with the dependency injection container using its own type as the key.
        /// </summary>
        /// <param name="implementationFactory"></param>
        /// <typeparam name="THostedService"></typeparam>
        /// <returns></returns>
        public IServiceCollection AddHostedServiceAsSelf<THostedService>(Func<IServiceProvider, THostedService> implementationFactory)
            where THostedService : class, IHostedService
        {
            services.AddSingleton<THostedService>(implementationFactory);
            services.AddHostedService<THostedService>(sp => sp.GetRequiredService<THostedService>());

            return services;
        }
    }
}
