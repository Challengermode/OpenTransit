using MassTransit;
using Shared;

namespace OrderService.Consumers;

public class SubmitOrderConsumer : IConsumer<SubmitOrder>
{
    public async Task Consume(ConsumeContext<SubmitOrder> context)
    {
        Console.WriteLine($"Inside SubmitOrderConsumer. Consuming the Message, OrderId: {context.Message.OrderId}");

        await Task.Delay(5000);

        var processOrder = new ProcessOrder
        {
            OrderId = context.Message.OrderId,
            ExtensionData = context.Message.ExtensionData // Propagating additional properties via ExtensionData
        };
        await context.Publish(processOrder);
        Console.WriteLine("ProcessOrder Message is published.");
    }
}
