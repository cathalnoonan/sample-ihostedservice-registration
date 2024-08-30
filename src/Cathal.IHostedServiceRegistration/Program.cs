using Cathal.IHostedServiceRegistration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedServiceAsSelf<MyHostedService>();

var host = builder.Build();
host.Run();
