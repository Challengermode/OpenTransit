
## Defining Topology

The term Topology is used in many contexts and has different meanings in different contexts. 

However, in a distributed system, it is generally used in two Contexts. They are, 
1. Broker
2. Architectural pattern. 

- In the context of a **broker**, it means the broker’s internal routing configuration i.e. the route mapping between the Publish Endpoint(i.e. Topics, Exchanges, etc) to Receive Endpoint(i.e Queues). 

- In the context of **Architectural patterns**, the Producers, Consumers and the Broker consists of the Topology. 

OpenTransit’s definition combines both perspectives. Because it not only helps you configure the broker’s internal topology, but also defines how your application (via OpenTransit) interacts with the broker—for example, how messages are produced and consumed.

Therefore, in OpenTransit:

- **Topology** refers to both the broker’s internal configuration and the application-level configuration that defines how it interacts with the broker.


However, when referring strictly to the broker-level routing configuration, this documentation will specifically use the term **“Broker’s Internal Topology.”**



## Configuring the Topology:
Now that we understand what topology means, let’s look at how to **configure** it.

Based on the definition, configuring topology involves two aspects:

- Configuring the broker’s internal setup, such as Publish Endpoints (topics, exchanges), Receive Endpoints (queues), routing, and so on.  
- Configuring how OpenTransit interacts with the broker.

OpenTransit defines topology through the **Message Type**.

### Message Types as First-class citizens:
You have already seen in the [tutorial](../tutorials/basic-communication#generic-broker-topology) how Message Types (classes, records, or interfaces) are used when Configuring the broker, 
and how Producers and Consumers interact with the Publish Endpoints and Receive Endpoints depending on the Message Type. 

Using message types provides strong typing, which you wouldn’t get if you were interacting directly with the broker’s raw .NET client.
It also abstracts away the complexity of broker topology and offers a method-call–like syntax that hides low-level broker communication details.

However, OpenTransit is flexible and also provides lower-level APIs when you need more direct control.

OpenTransit provides two ways to define the broker topology.

- Broker Agnostic way(Common for any broker)
- Broker Specific way(features provided by the broker, for example, routing key by RabbitMQ)


## Broker agnostic Way:
We can define both the Broker’s internal Configuraiton and OpenTransit’s Interaction in a Broker agnostic way. 

In the [tutorial](../tutorials/basic-communication.md#generic-broker-topology), the topology is defined in a broker agnostic way. 

One of the most powerful feature of OpenTransit is the way it abstracts away the detail of a broker. 
We can use an underlying broker without knowing the intrinsic details. 

How Masstransit defines the abstraction and what are the benefits is discussed in the [Generic Broker](generic-broker.md).

The abstraction provides many benefits and most of the time it provides the feature we needs so we should choose the Broker agnostic configuration whenever possible. 


## Broker Specific Way:
Though broker agnostic configuration is recommended, however, in some scenarios, we may have some special requirements and need to use some special features that is provided by a specific broker. 
OpenTransit is flexible and provides a way where you can define broker specific features. 

> [!NOTE]
> We will add tutorials/examples later. 






### Terminologies:

You may get confused with the usage of these terms in the doc with the terms used on Producer Consumer or Pub-Sub pattern. 

Well, in OpenTransit, we generally use the terms in our own way which may sometimes not exactly the same as their definition in other contexts. 

For example, 

- Producer **Publishes** Messages to the Publish Endpoint
- Producer **Sends** Messages directly to the Receive Endpoint
- Consumer **Subscribes** to a Receive Endpoint
- Consumer **Consumes** Messages

Let us discuss a bit more about them

#### Producer

What we mean by Producer depends on the Context. 

When we discuss **Topology** or **Application**, a Producer is an **Application** that Publishes or Sends Messages to Publish Endpoints and Receive Endpoints respectively. 
In the [Basic Communication Tutorial](../tutorials/basic-communication.md#introducing-the-services) the Client and OrderService are Producer Applications.


However, when we discuss **Code**, a Producer is a **type(ex, Class)** that implements specific interfaces that contain Send and/or Publish methods to Publish or Send messages to Publish Endpoints or Receive Endpoints respectively. 

For example, `IBus`, `ConsumeContext`, etc are Producers. See the [Basic Communication Tutorial](../tutorials/basic-communication.md#2-publishing-messages) 

We will discuss the Producer in more detail in a dedicated section. 


#### Consumer

When we discuss **Topology** or **Application**, a Consumer is an **Application** that Consumes Messages from Receive Endpoint(s). 
In the [Basic Communication Tutorial](../tutorials/basic-communication.md#introducing-the-services), the InventoryService and OrderService are Consumer Applications.

When we discuss **Code**, a Consumer is an implementation of `IConsumer<T>` where `T` is a Message Type. 
In the [Basic Communication Tutorial](../tutorials/basic-communication.md), `SubmitOrderConsumer` and `ProcessOrderConsumer` are implementation of Consumers


We will discuss Consumers in more detail in a dedicated section. 











