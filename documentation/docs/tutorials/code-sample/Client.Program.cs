using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

var host = builder.Build();

var applicationLifetimeService = host.Services.GetService<IHostApplicationLifetime>();
applicationLifetimeService?.ApplicationStarted.Register(async void (_, _) =>
{
    var bus = host.Services.GetService<IBus>();

    var submitOrderMessage = new SubmitOrder { OrderId = "123" };

    if (bus is not null)
    {
        await bus.Publish(submitOrderMessage);
        Console.WriteLine("SubmitOrder Message is published");
    }
}, null);

host.Run();
