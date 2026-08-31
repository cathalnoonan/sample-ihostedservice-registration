# IHostedService Registration

By default, the implementation used when registering an `IHostedService` with the Microsoft.Extensions.DependencyInjection ServiceCollection is not registered as the service type.

Instead, all hosted services added using `.AddHostedService<THostedService>()` are registered as implementations of `IHostedService` ([see implementations here](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/Microsoft.Extensions.Hosting.Abstractions/src/ServiceCollectionHostedServiceExtensions.cs#L25)).

This makes it awkward to retrieve a specific hosted service from the `IServiceProvider`.

For example:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class MyHostedService : IHostedService
{
    // omitted for brevity
}

using var serviceProvider = new ServiceCollection()
    .AddHostedService<MyHostedService>()
    //.AddHostedService<MyHostedService>(sp => new MyHostedService(...))
    .BuildServiceProvider();

// Retrieving the hosted service from the IServiceProvider...
var allHostedServices = serviceProvider.GetServices<IHostedService>(); // OK
var myHostedService = serviceProvider.GetService<MyHostedService>();   // Null
```

---

## Solution

To allow a way to retrieve specific hosted services easily, add extension methods to your project that wrap the `.AddHostedService<THostedService>()` method (and any overloads as necessary).

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
```

After these extension methods have been added, register and retrieve the hosted service from the IServiceProvider as follows:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class MyHostedService : IHostedService
{
    // omitted for brevity
}

using var serviceProvider = new ServiceCollection()
    .AddHostedServiceAsSelf<MyHostedService>()
    //.AddHostedServiceAsSelf<MyHostedService>(sp => new MyHostedService(...))
    .BuildServiceProvider();

// Retrieving the hosted service from the IServiceProvider...
var allHostedServices = serviceProvider.GetServices<IHostedService>(); // OK
var myHostedService = serviceProvider.GetService<MyHostedService>();   // OK
```
