using MassTransit;
using Shared;

namespace InventoryService.Consumers;

public class ProcessOrderConsumer : IConsumer<ProcessOrder>
{
    public Task Consume(ConsumeContext<ProcessOrder> context)
    {
        Console.WriteLine($"Inside ProcessOrder Consumer: Consuming the Message. OrderId: {context.Message.OrderId}");
        return Task.CompletedTask;
    }
}
