
The generic broker concept is an abstraction of all the Broker supported by Masstransit. It contains all the common features of all the supported brokers. 

# Common Broker Features

Different message brokers provide different capabilities, but if you look at the most popular ones—RabbitMQ, Azure Service Bus, ActiveMQ, and Amazon SQS—you’ll notice a shared set of core features.  
In practice, these common features cover the majority of real-world use cases.

For example:

- All of them offer a **Publish Endpoint** where messages can be published.  
  - RabbitMQ calls this an **Exchange**.  
  - Azure Service Bus, ActiveMQ, and Amazon SQS call it a **Topic**.

- All of them provide a **Receive Endpoint**, typically referred to as a **Queue**, where consumers subscribe.

Producers publish messages to Publish Endpoints, and Consumers subscribe to Receive Endpoints.

Most brokers also allow you to configure **routing** between Publish Endpoints and Receive Endpoints.  
This means that when a message arrives at a Publish Endpoint, the broker forwards it to one or more Receive Endpoints based on the defined routing configuration.  
(You can still send messages directly to the ReceiveEndpoint if needed.)

These behaviors are consistent across brokers.



# The Generic Broker Concept

OpenTransit takes advantage of these shared capabilities by introducing a **Generic Broker**—an abstraction that represents the common behavior of message brokers.  
You configure your topology against this Generic Broker, and OpenTransit translates that configuration into the appropriate broker-specific topology under the hood.

This abstraction is the foundation of the Generic Broker concept.


## Generic Broker Terminologies

A Generic Broker defines two types of endpoints:

- **Publish Endpoints**  
- **Receive Endpoints**

You can create and configure the routing between PublishEndpoint to the ReceiveEndpoint based on Message Types.

- Messages are generally **published** to a Publish Endpoint.  
- Consumers **subscribe** to Receive Endpoints.  
- Messages may also be sent **directly** to a Receive Endpoint if needed.

You can define **routing mappings** between endpoints—specifying which Receive Endpoint(s) should receive messages published to a given Publish Endpoint.

In the [Basic Communication Tutorial](../tutorials/basic-communication.md#generic-broker-topology) we have seen how Message types is used to Create and Configure Publish Endpoints and Receive Endpoints. 

> [!NOTE]
> Some concepts in this documentation are still evolving. Details on how the Generic Broker constructs the underlying topology for different message types will be added in a future update.


