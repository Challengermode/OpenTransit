# The Abstraction is the way

So you want to build a Distributed Communication framework for your system.  
You’ve probably already chosen a message broker — RabbitMQ, Azure Service Bus, Amazon SQS, or something else.

But here comes the million-dollar question: should you build the framework from scratch for your distributed application?
That’s a lot of infrastructure-layer code. And there’s a high chance that instead of building the next unicorn with your brilliant idea, you’ll end up spending your days battling infrastructure issues.

What if you could use a framework that abstracts away all the details of broker communication and provides a clean, consistent programming model?
A framework that lets you focus on your business logic instead of worrying about message serialization, retries, error handling, or distributed transactions, etc. 

That’s exactly where OpenTransit comes in.
It provides a battle-tested foundation for message-based systems, handling the complexities of message routing, sagas, retries, and fault tolerance — so you can stay focused on your core business logic and move faster with confidence.

OpenTransit handles all the complexities behind the scenes, providing a clear and consistent interface for your messages, while still allowing you to configure broker-specific settings whenever needed. 

Next, let’s take a closer look at how it abstracts the broker and simplifies distributed communication.



## Abstracting the Broker

Almost all the brokers provide some Basic Functionality. And most of the time the generic functionalities is sufficient for our use cases.  

OpenTransit introduces a concept of *[Generic Broker](/Topology%20&%20Generic%20Broker/Generic%20Broker)*. With this, we can configure topology of the distributed without knowing the internal details of the broker. 

However, we can use broker specific features too if we need. 




## Patterns
OpenTransit implements several Distributed System Communication Patterns that you can use out of the box.    
These patterns simplify complex messaging and orchestration scenarios, helping you build robust and maintainable distributed applications.

Some of the supported patterns include:

- Request Response Pattern
- Routing Slip Pattern
- Mediator
- Saga
- Outbox

etc. 

By providing these patterns natively, OpenTransit helps you focus on your business logic instead of reinventing common messaging workflows.



