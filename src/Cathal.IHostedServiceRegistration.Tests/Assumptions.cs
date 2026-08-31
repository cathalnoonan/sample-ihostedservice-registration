using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cathal.IHostedServiceRegistration.Tests;

internal class Assumptions
{
    [Test]
    public void AddHostedService_RegistersSingleton()
    {
        var serviceDescriptors = new ServiceCollection()
            .AddHostedService<MyHostedService>()
            .ToList();

        Assert.That(serviceDescriptors, Has.Count.EqualTo(1));
        var descriptor = serviceDescriptors[0];
        using (Assert.EnterMultipleScope())
        {
            Assert.That(descriptor.ServiceType, Is.EqualTo(typeof(IHostedService)));
            Assert.That(descriptor.Lifetime, Is.EqualTo(ServiceLifetime.Singleton));
        }
    }
}
