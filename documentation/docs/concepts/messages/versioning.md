# Message Versioning

<br/>

> [!Note] 
> OpenTransit does not impose any opinion on how message contracts should be handled as they evolve over time. That responsibility is entirely yours.
That said, this documentation outlines recommended guidelines you may choose to follow.

As an application’s business logic evolves, its message contracts may need to evolve as well. 
For example, properties may be added, removed, or renamed within a message type.

When such changes occur, the class library containing the message contract is updated and a new NuGet version is published. 
The updated message contract package is then referenced **only** by the producer and/or consumer applications that **require** the new behavior, 
along with the corresponding business logic changes, then they are deployed.

> [!Important] 
> For proper versioning and long-term maintainability, it is recommended to use a **shared class library as NuGet package** that contains the message contracts. 
Throughout this documentation, we assume this approach is being followed.

Since not all applications are updated at the same time, the system can end up running with ***multiple versions*** of the same message contract simultaneously.

This concept, **message multi-versioning** must be handled with great care, as it can easily lead to system failures. 
For example, a newer consumer may treat a property as required, while an older producer does not send that property at all. 
In such cases, the consumer may fail at runtime due to missing data.

This is why proper message versioning is critical in distributed systems.

In the following sections, we will analyze several hypothetical examples.
By carefully reviewing these examples, you should be able to handle message versioning correctly in most real-world situations.

If you encounter an edge case or scenario that is not covered here, feel free to discuss it with us. We will incorporate relevant findings into the documentation.

---

<br/>

## The Scenarios

There is a Producer Application P1 that publishes an Event Message of Type M. Two Consumer Applications, C1 and C2, both consume the Event Message M.

We will explain 3 scenarios: **adding**, **renaming**, and **removing** a property on a Message Contract. 

<br/>

### Scenario 1: Adding an extra Property

Let’s say the business evolves, and the C1 consumer needs an extra parameter.
So a new property is added to M, NuGet is published, both P1 and C1’s NuGet packages are updated, business logic is added, and deployment is done. 

However, C2 has no business with the new property, so it isn’t redeployed and continues to use the old contract version, which doesn't include the newly added property. 

So what could go wrong after P1 and C1’s new deployment? And how to solve. Let’s see.

<br/>

##### The Publisher P1(with updated Message Contract)

Publishing isn’t an issue. It always works. Problems happen with Consumers. 

<br/>

##### The Consumer C2(with old Message Contract)

The Consumer C2 will work perfectly fine. 
Yes, it will receive the message M with an extra property. However, getting extra property also isn’t an issue. The serializer will simply ignore it. 
Since C2 has no business with that extra property, everything works fine.

<br/>

##### The Consumer C1(with updated Message Contract)

If, after the deployment, the Consumer C1 gets any **Old** message(that doesn’t contain the new property), 
it will break unless the newly added Property on the Message Type is **nullable**. 

**How C1 may get an old message?**

- There were already some old messages in the broker. As soon as the new C1 deployment starts consuming, it will retrieve those old messages. 
- Even if there are no old messages on the broker, when you deploy two applications in parallel, for example, in Kubernetes, the P1 rollout may complete later than C1.

<br/>
<br/>


### Scenario 2: Renaming a Property

Now imagine the same Scenario as before, however, instead of adding a new property, an existing property is renamed. 

What could go wrong after P1 and C1’s new deployment? And how to solve it. Let’s see. 

##### The Publisher P1(with updated Message Contract)

Publishing, as always, works fine. 

<br/>

##### The Consumer C2(with old Message Contract)

The Consumer C2 is using the old contract. So it won’t recognize the **new** property name and will simply ignore it on deserialization. 

However, the **old** property value will be null for new messages, which will cause an issue. 

(However, if the **old** property value was **nullable** and sending null doesn’t hamper the business, then you are okay.) 

<br/>

##### The Consumer C1(with updated Message Contract)

The Consumer C1 will break if it gets any **old** message because the old message doesn’t contain the renamed property. 

However, ([if the serializer supports](https://stackoverflow.com/questions/66416800/mapping-multiple-json-property-names-to-the-same-property-in-system-text-json)) 
this can be easily solved by instructing the serializer to look for both the old and the renamed value when deserializing by adding a private property on the message.  

If the serializer trick isn't possible then the renamed property should be made **nullable**(if business allows) to avoid breaking C1 when it gets old messages.

<br/>
<br/>

### Scenario 3: Removing a Property

Same scenario as above, but here we will **remove** a property rather than add or rename one. Let’s see what happens. 

<br/>

##### The Publisher P1(with updated Message Contract)

Publishing, as always, works fine.

<br/>

##### The Consumer C2(with old Message Contract)

Will break (unless the removed property was **nullable** in the old contract and a null value doesn’t affect the business). 

<br/>

##### The Consumer C1(with updated Message Contract)

Old messages will have the removed property; however, the serializer will simply ignore it. 
So if the removed property was not critical to the business logic for the old messages, everything will work fine.

<br/>
<br/>

### Conclusion

So here we tried to cover some common scenarios that may arise when versioning message contracts and possible solutions(if existing) to avoid breaking consumers
so that in the production environment, you can have some idea of what could go wrong and what measures you need to take.

---

<br/>

## Propagation Trick

It’s a serialization trick that allows you to propagate a new message version without requiring changes/NuGet update in the propagator applications.

So what are propagator applications?

Well, in OpenTransit, some ***producers*** act as **initiators**, while others act as **propagators**.
Propagators are producers that publish or send messages from **within** a Consumer.

Examples are better than definitions, 
so we’ll explain this propagation technique using the [Basic Communication Tutorial](../../tutorials/basic-communication.md) example.
However, just imagine that the **Shared** class library containing the MessageContract isn’t a project reference; instead, it is published as a NuGet package.
(In the example, we added it as a project reference for simplicity).

If you recall the [Basic Communication Tutorial](../../tutorials/basic-communication.md#2-publishing-messages), 
the `ProcessOrder` message is published from within the **SubmitOrderConsumer**. 

So here **SubmitOrderConsumer’s** application is a **Propagator** for the `ProcessOrder` message.

**Now let’s look at the trick.**

Assume the business evolves and, when placing an order, we also capture an **expected shipping date**, 
which is ultimately used by the **InventoryService** (inside **ProcessOrderConsumer**).

This new requirement impacts only the **Client** and the **InventoryService**.  
So we add a new nullable property, `DateTime? ExpectedShippingDate`, to both the `SubmitOrder` and `ProcessOrder` message types and publish a new version of the contract. 
We then update and deploy the **Client** and the **InventoryService**.

At this point, if the Client publishes a message that includes `ExpectedShippingDate`, will **ProcessOrderConsumer** eventually receive it?

**The answer is 'No'.**

Because the **OrderService** Propagator Application has not been updated and is still using the older version of the `SubmitOrder` contract, 
the `ExpectedShippingDate` value is discarded during deserialization and never makes it downstream.

### The naive solution

The naive approach is to update the Message Contract of **OrderService** and manually propagate the `ExpectedShippingDate` property and redeploy.

[!code-csharp[](code-samples/OrderService.SubmitOrderConsumer-Naive.cs#L6-L122)]

While this approach works, it introduces unnecessary overhead. Even though no business logic of **OrderService** has changed, 
the application still needs to be redeployed just to propagate a new property.

In real-world systems, the message propagation chain can be much longer. In such cases, every intermediary (propagator) application would also need to be updated and redeployed, 
amplifying the operational complexity.


### The Propagation Trick Solution

We can solve this by doing the Propagation trick. With this trick, 
we can design the messages and propagators from the beginning in a way that propagators need not be updated to propagate newly added properties. 

It is actually a serialization trick. 

If we use System.Text.Json, we can add an additional property `ExtensionData` of type `Dictionary<string, JsonElement>?` with `JsonExtensionData` attirbute to both the 
`SubmitOrder` and `ProcessOrder` classes.

And then propagate the ExtensionData property on the propagators. 

[!code-csharp[](code-samples/OrderService.SubmitOrderConsumer-Trick.cs#L6-L122)]

If we design the messages and propgators this way from the beginning, then when a new property is added later, the propagator applications need **not** be updated to propagate that new property. 

That is because on Deserialization, System.Text.Json will put all unknown properties in the `ExtensionData` dictionary, and on Serialization, it will write all key-value pairs from the `ExtensionData` dictionary as normal properties.
So here all we need to do is copy the `ExtensionData` dictionary from the consumed message to the propagated message.

You may know more about `JsonExtensionData` [here](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/handle-overflow).

Chris Patterson has similar example [here](https://www.youtube.com/watch?v=PNNxJthctgk) where the data is sent to the frontend .

---

<br/>

## Enum issues

Enums should be used with caution, as they aren’t backward compatible. If you add a new key to the enum but the Consumer isn’t updated, it will throw an exception. 

Chris Patterson has explained this [here](https://www.youtube.com/watch?v=PNNxJthctgk&t=589s).

---


