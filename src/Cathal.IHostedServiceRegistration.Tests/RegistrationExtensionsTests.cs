using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Cathal.IHostedServiceRegistration.Tests;

public class Tests
{
    private readonly NullLoggerFactory _loggerFactory = new();

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _loggerFactory.Dispose();
    }

    [Test]
    public void ShouldRegisterHostedServiceAsSelf()
    {
        using var serviceProvider = new ServiceCollection()
            .AddHostedServiceAsSelf<MyHostedService>()
            .AddLogging()
            .BuildServiceProvider();

        var myHostedService = serviceProvider.GetService<MyHostedService>();
        var allServices = serviceProvider.GetServices<IHostedService>()?.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(myHostedService, Is.Not.Null);
            Assert.That(allServices, Is.Not.Null);
            Assert.That(allServices, Has.Count.EqualTo(1));
            Assert.That(allServices![0], Is.InstanceOf<MyHostedService>());
            Assert.That(allServices![0], Is.EqualTo(myHostedService));
        });
    }

    [Test]
    public void ShouldRegisterHostedServiceAsSelf_WithImplementationFactory()
    {
        using var serviceProvider = new ServiceCollection()
            .AddHostedServiceAsSelf<MyHostedService>(sp => new MyHostedService(_loggerFactory.CreateLogger<MyHostedService>()))
            .BuildServiceProvider();

        var myHostedService = serviceProvider.GetService<MyHostedService>();
        var allServices = serviceProvider.GetServices<IHostedService>()?.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(myHostedService, Is.Not.Null);
            Assert.That(allServices, Is.Not.Null);
            Assert.That(allServices, Has.Count.EqualTo(1));
            Assert.That(allServices![0], Is.InstanceOf<MyHostedService>());
            Assert.That(allServices![0], Is.EqualTo(myHostedService));
        });
    }
}
