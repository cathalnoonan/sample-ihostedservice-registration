namespace Cathal.IHostedServiceRegistration;

public class MyHostedService : BackgroundService
{
    private readonly ILogger<MyHostedService> _logger;

    public MyHostedService(ILogger<MyHostedService> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MyHostedService is running.");
        return Task.CompletedTask;
    }
}
