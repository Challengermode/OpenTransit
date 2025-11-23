
The generic broker concept is an abstraction of all the Broker supported by OpenTransit. It contains all the common features of all the supported brokers. 

## Common Broker Features

Different message brokers provide different capabilities, but if you look at the most popular ones—RabbitMQ, Azure Service Bus, ActiveMQ, and Amazon SQS & SNS, you’ll notice a shared set of core features.  
In practice, these common features cover the majority of real-world use cases.

For example:

- All of them offer a **Publish Endpoint** where messages can be published.  
  - RabbitMQ calls this an **Exchange**.  
  - Azure Service Bus, ActiveMQ, and Amazon SNS call it a **Topic**.

- All of them provide a **Receive Endpoint**, typically referred to as a **Queue**, where consumers subscribe.

Producers publish messages to Publish Endpoints, and Consumers subscribe to Receive Endpoints.

Most brokers also allow you to configure **routing** between Publish Endpoints and Receive Endpoints.  
This means that when a message arrives at a Publish Endpoint, the broker forwards it to one or more Receive Endpoints based on the defined routing configuration.  
(You can still send messages directly to the ReceiveEndpoint if needed.)

These behaviors are consistent across brokers.



## The Generic Broker Concept

OpenTransit takes advantage of these shared capabilities by introducing a **Generic Broker**—an abstraction that represents the common behavior of message brokers.  
You configure your topology against this Generic Broker, and OpenTransit translates that configuration into the appropriate broker-specific topology under the hood.

This abstraction is the foundation of the Generic Broker concept.


### Endpoints

A Generic Broker defines two types of endpoints:

- **Receive Endpoint**
- **Publish Endpoint**  

#### Receive Endpoint

A Receive Endpoint of a Broker is where Consumer(s) can Subscribe to Consume Messages. 

Messages can also be sent directly to the Receive Endpoint, too(instead of publishing to the Publish Endpoint). 
This approach is typically used when performance is critical and no complex routing is required. We will discuss it in a separate section.

The Generic Broker’s Receive Endpoint creates a [point-to-point Channel](https://www.enterpriseintegrationpatterns.com/patterns/messaging/PointToPointChannel.html) with its Consumers. 
This means that even if **multiple** Consumers are attached to the **same** Receive Endpoint, each message is delivered to **only** one of them([Competing Consumer Pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html)).

#### Publish Endpoint

A Publish Endpoint of a broker is where Publishers Can Publish Messages. 

#### Routing 

When a Message is published on a Publish Endpoint, it is routed to the appropriate Receive Endpoint(s) based on the [topology’s](topology.md#configuring-the-topology) routing configuration. 

In the [Commands and Events Pattern](../patterns/commands-and-events/overview.md#generic-broker-topology), we see how Message types are used to Create and Configure Publish Endpoints and Receive Endpoints of the Generic Broker. 






> [!NOTE]
> Some concepts in this documentation are still evolving. Details on how the Generic Broker constructs the underlying topology for different message types will be added in a future update.


