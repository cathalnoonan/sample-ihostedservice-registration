using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;

namespace Cathal.IHostedServiceRegistration.Tests;

internal class Tests
{
    [Test]
    public void ShouldRegisterHostedServiceAsSelf()
    {
        using var serviceProvider = new ServiceCollection()
            .AddHostedServiceAsSelf<MyHostedService>()
            .AddLogging()
            .BuildServiceProvider();

        var myHostedService = serviceProvider.GetService<MyHostedService>();
        var allServices = serviceProvider.GetServices<IHostedService>()?.ToList();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(myHostedService, Is.Not.Null);
            Assert.That(allServices, Is.Not.Null);
            Assert.That(allServices, Has.Count.EqualTo(1));
            Assert.That(allServices![0], Is.InstanceOf<MyHostedService>());
            Assert.That(allServices![0], Is.EqualTo(myHostedService));
        }
    }

    [Test]
    public void ShouldRegisterHostedServiceAsSelf_WithImplementationFactory()
    {
        using var serviceProvider = new ServiceCollection()
            .AddHostedServiceAsSelf<MyHostedService>(sp => new MyHostedService(NullLogger<MyHostedService>.Instance))
            .BuildServiceProvider();

        var myHostedService = serviceProvider.GetService<MyHostedService>();
        var allServices = serviceProvider.GetServices<IHostedService>()?.ToList();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(myHostedService, Is.Not.Null);
            Assert.That(allServices, Is.Not.Null);
            Assert.That(allServices, Has.Count.EqualTo(1));
            Assert.That(allServices![0], Is.InstanceOf<MyHostedService>());
            Assert.That(allServices![0], Is.EqualTo(myHostedService));
        }
    }
}
