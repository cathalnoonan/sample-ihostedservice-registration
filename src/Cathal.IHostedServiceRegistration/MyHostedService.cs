namespace Cathal.IHostedServiceRegistration;

public class MyHostedService(ILogger<MyHostedService> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("MyHostedService is running.");
        return Task.CompletedTask;
    }
}
