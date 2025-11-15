
In this tutorial, we will explore the basics of publishing and consuming messages.
The source code for this example is available **[here](https://github.com/OpenTransitLab/Tutorials/tree/main/Tutorials.BasicCommunication)**


## Introducing the Services
In this tutorial, we’ll work with three services:

- **Client** – A simple console application that publishes a message to simulate an order arriving from the frontend.
- **OrderService** – Handles incoming orders. After processing an order, it publishes another message to continue the workflow.
- **InventoryService** – Listens for order-processing–related messages and handles the inventory side of the workflow.

In this setup:
- The **Client** acts as a **Producer** — it only publishes messages.  
- The **InventoryService** acts as a **Consumer** — it only receives messages.  
- The **OrderService** is both a **Producer** and a **Consumer** — it consumes one message and then publishes another.


## Explanation
You need to understand these 5 things to have a basic knowledge of Distributed Communication with OpenTransit. 

1. [Defining Messages](#1-defining-messages)
2. [How Messages are published](#2-publishing-messages)
3. [How Messages are Consumed](#3-consuming-a-message)
4. [How the OpenTransit Setup is done](#4-masstransit-setup) 
5. [The Topology Created inside the Broker](#5-the-broker-topologyrabbitmq)


### 1. Defining Messages

Messages are the means of communication between Services. They can be defined using **classes**, **interfaces**, or **records**.

In this project, we define two messages using C# classes: **SubmitOrder** and **ProcessOrder**.

- The **Client** publishes a **SubmitOrder** message.  
- The **OrderService** consumes **SubmitOrder**, processes it, and then publishes **ProcessOrder**.  
- The **InventoryService** consumes **ProcessOrder**.

#### Sharing Message Types Across Projects

When a message is used by multiple applications—such as a producer and a consumer—they **must use the exact same namespace** for the message type.  

For example:

- **SubmitOrder** is used by both Client and OrderService → same namespace required  
- **ProcessOrder** is used by OrderService and InventoryService → same namespace required

To simplify this, we created a **shared class library** that contains the message definitions. This is the easiest and most maintainable approach.

However, using a shared library is **not mandatory**. Each project may define its own copy of the message class, as long as the **namespace matches exactly**, ensuring that the broker treats them as the same message type.

---

### 2. Publishing Messages

Messages are published by **Producers**. A Producer exposes a `Publish(T message)` method, which is used to send messages to the broker.  
(We’ll cover Producers in detail later in the documentation.)

In this example, messages are published from two places:

- From the **Client** project’s `Program.cs`  
[!code-csharp[](code-sample/Client.Program.cs#L25-L33)]


- From inside the **SubmitOrderConsumer**
[!code-csharp[](code-sample/OrderService.SubmitOrderConsumer.cs#L6-L18)]

Both `IBus` and `ConsumeContext<T>` act as Producers, because you can call `Publish(T message)` on either of them.

When publishing inside a [Consumer](#publishing-from-inside-a-consumer), it is **highly recommended** to use the `ConsumeContext<T>` Producer as we have done in the SubmitOrderConsumer. We’ll discuss why in the dedicated Producers section.

When publishing *outside* a Consumer, you can resolve the `IBus` service and publish messages using it like we have done in the **Client** Project's `Program.cs`.

In OpenTransit, the necessary services (such as `IBus`) are registered in `Program.cs`(See the [Setup](#4-masstransit-setup), allowing you to resolve and use them throughout the application.

---

### 3. Consuming a Message:

To consume a message of type `T`, you need to implement `IConsumer<T>`.

In our example, we have two consumers:

- **SubmitOrderConsumer** in the **OrderService**  
[!code-csharp[](code-sample/OrderService.SubmitOrderConsumer.cs#L6-L18)]
- **ProcessOrderConsumer** in the **InventoryService**
[!code-csharp[](code-sample/InventoryService.ProcessOrderConsumer.cs#L6-L13)]

Each consumer must also be registered in the OpenTransit [Setup](#4-masstransit-setup) so the framework knows to create and connect them to the message pipeline.

#### Publishing from inside a Consumer

The `IConsumer<T>` defines a `Consume` method which is called when a message of type `T` is consumed. The method passes a Producer namely `ConsumeContext<T>`, it is highly recommended to use this producer(`ConsumeContext`) when we publish messages from inside the consumer. As you see we have done it in the SubmitOrderConsumer. 

---


### 4. MassTransit Setup:
We configure OpenTransit in the **Service Registration** section of `Program.cs`.  
In this step, we perform three main tasks:

1. **Register the required services**  
   Simply call `builder.Services.AddMassTransit()` and it will register everything needed.

2. **Configure the connection to the message broker**

3. **Register the consumers**(if any)

All the above 3 tasks is done onthe `Program.cs` of OrderService
[!code-csharp[](code-sample/OrderService.Program.cs#L12-L25)]


---

### 5. The Broker Topology(RabbitMQ):

This example uses **[broker-agnostic](../concepts/topology#broker-agnostic-way)** configuration, so you don’t need to understand the underlying broker [topology](../concepts/topology) for basic communication.  
Here, we only used the `UsingRabbitMq` method to provide the Connection Configuration and to configure the [ReceiveEndpoint](../concepts/generic-broker#generic-broker-terminologies)(a generic concept among all the brokers) on the broker. 

Since this Configurations aren't RabbitMQ specific, and you may use any other broker here and, the Message Communication would work fine. 

However, knowing the underlying broker [topology](../concepts/topology) is very helpful when debugging message-routing issues.

In our example, Each Consumer is consuming a single message type. 
Here, for each MessageType, 2 **Exchanges** and one **Queue** are being created. 

For example, if you open the RabbitMq management plugin, you will see, for the SubmitOrder MessageType, a **Queue** named `SubmitOrder` is bound to the **Exchange** Named `SubmitOrder`. 
Another **Exchange** named `Shared:SubmitOrder` is created and the `SubmitOrder` **Exchange** is bound to it. (Here, **'Shared'** came from the **Namespace** of the message)

When we publish SubmitOrder messages via a Producer, the message is published to the `Shared:SubmitOrder` exchange, then routed to the SubmitOrder exchange and, then ultimately routed to the SubmitOrder queue. 


#### Generic Broker's Perspective
From the perspective of the [Generic Broker](../concepts/generic-broker), the `Shared:SubmitOrder` **Exchange** is the PublishEndpoint, the `SubmitOrder` **Queue** is the ReceiveEndpoint. 
The `SubmitOrder` **Exchange** can be seen as an Internal ReceiveEndpoint. However, we haven't added such concept/term in the [Generic Broker](../concepts/generic-broker) yet.  


Why the topology is defined that way, what we are gaining, etc, is out of scope of this tutorial. We will have separate sections for it. 

To have a better understanding, you may clone the [project](https://github.com/OpenTransitLab/Tutorials/tree/main/Tutorials.BasicCommunication) and create more message types experimentation.



