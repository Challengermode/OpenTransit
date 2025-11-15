The term Topology is used in many context and have different meaning in different context. 

However, in distributed system, it is generally used in two Contexts. They are, 
1. Broker
2. Architectural pattern. 

In the context of a **broker**, it means the broker’s internal routing configuration i.e. the route mapping between the Publish Endpoint(i.e. Topics, Exchanges, etc) to Receive Endpoint(i.e Queues). 

In the context of **Architectural patterns**, the Producers, Consumers and the Broker consists of the Topology. 

Opentransit’s Definition is sort of a combination of the both cause opentransit helps you not only to Configure the Broker internally, but also helps to interact with it(i.e to Produce, consume, etc). 

So in OpenTransit, **Topology is the broker’s internal configuration, and also the Configuration of how the Application (via OpenTransit) interacts with the broker.**

However, when we want to mean only in broker's context, we will use the term 'Broker's Underlying Topology' throughout the doc. 



## Configuring the Topology:
So we now know what a topology is. Now we will know how to Configure the topology. 

From the definition, Configuring topology may mean Configuring both of the following things, 

- Configuring the broker’s internal configuration like Publishendpoints(topic, exchange), Receive Endpoints(i.e. queue),  routing, etc. 
- Configuring how OpenTransit interacts with the broker. 

OpenTransit defines topology via the Message type 

### Message Types as First-class citizens:
You have already seen in the [tutorial](../tutorials/basic-communication) how Message Types (classes, records, or interfaces) are used when Configuring the broker, and how Producers and Consumers interact with the PublishEndpoint and ReceiveEndpoint depending on the Message Type. 
It gives a strongly typed facility that you wouldn’t have found if you were to use the raw .NET Client of the broker. 
It also abstracts away the details of topology from the Broker interaction point of view and provides a Method call-like syntax and hides the detail of broker Communication. 

However, OpenTransit is flexible enough to provide lower level API’s if we want a lower level interaction.

OpenTransit provides two ways to define the broker topology.

- Broker Agnostic way(Common for any broker)
- Broker Specific way(features provided by the broker, for example, routing key by RabbitMQ)(give an example link)


## Broker agnostic Way:
We can define both the Broker’s internal Configuraiton and OpenTransit’s Interaction in a Broker agnostic way. 

In the [tutorial](../tutorials/basic-communication.md), the topology is defined in a broker agnostic way. 

One of the most powerful feature of OpenTransit is the way it abstracts away the detail of a broker. 
We can use an underlying broker without knowing the intrinsic details. 

How Masstransit defines the abstraction and what are the benefits is discussed in the [Generic Broker](generic-broker.md).

The abstraction provides many benefits and most of the time it provides the feature we needs so we should choose the Broker agnostic configuration whenever possible. 


## Broker Specific Way:
Though broker agnostic configuration is recommended, however, in some scenarios, we may have some special requirements and need to use some special features that is provided by a specific broker. 
OpenTransit is flexible and provides a way where you can define broker specific features. 

> [!NOTE]
> We will add tutorials/examples later. 






### Producer, Consumer, Publish, Subscribe, etc:

You may get confused with the usage of these terms in the doc with the terms used on Producer Consumer or Pub-Sub pattern. 

Well, in OpenTransit, we generally use the terms in our own way which may sometimes not exactly the same as their definition in other contexts. 

For example, 

- Producer Publishes Messages to the PublishEndpoint
- Producer Sends Messages directly to the ReceiveEndpoint
- Consumer Subscribes to a Receive Endpoint
- Consumer Consumes Messages

Here,
Producer is something that Publishes/Sends Messages to the broker. 
Consumer is something that Consumes/Subscribes to a ReceiveEndpoint to Consume Messages.  

If two consumer subsribes to the Same ReceiveEndpoint, then only one will get a particular message(Competing Consumer pattern). So in that way it matches the Consumer’s definition of the Producer-Consumer pattern. 

Sometimes the terms Producer and Consumer is used to Describe the Application. 
For example, the Applicaiton that publishes messages is a Producer Application, the application that Consumes messages is a Consumer. 
However, an application may be both Producer and Consumer at the same time. 












