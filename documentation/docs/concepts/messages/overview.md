Messages are the means of Communication between services. Publishers publish messages, and Consumers consume. 

---

### Defining Message Contracts with CLR Types

OpenTransit supports **Classes**, **Records**, and **Interfaces** to define message Contracts. 

In message broker systems, internally, the message is serialized (e.g., to JSON) and transmitted over the network. 

However, since Opentransit provides strongly typed representations via Classes, Records, or Interfaces, 
it allows us to establish a clear contract between services and effectively manage [message versions](./versioning).


<br/>

Under the hood, OpenTransit not only handles serialization and deserialization but also performs the additional tasks required for distributed-system communication. 
This includes **wrapping messages in envelopes** and **attaching metadata** such as **source**, **destination**, **message type**, **correlation ID**, etc.

These responsibilities are encapsulated within the infrastructure layer, so in most cases, publishers and subscribers do not need to be concerned with them.
By abstracting away these infrastructure details, OpenTransit provides a clean, method-like syntax for publishing and consuming messages.
We will explore this in more detail in the Message Envelope section.

---

### Message Design Guidelines

When defining message contracts, what follows is general guidance based upon years of using MassTransit(till V8) combined with continued questions raised by developers new to 
MassTransit(till V8).

As we continue to evolve OpenTransit, we will refine these guidelines further.

> [!TIP]
> Use records, define properties as `public` and specify `{ get; init; }` accessors. Create messages using the constructor/object initializer or a message initializer. 

> [!TIP]
> Use interfaces, specify only `{ get; }` accessors. Create messages using message initializers and use the Roslyn Analyzer to identify missing or incompatible properties.

> [!WARNING]
> Message design is not object-oriented design. Messages should contain state, not behavior. Behavior should be in a separate class or service.

> [!WARNING]
> Limit the use of interface inheritance, pay attention to polymorphic message routing.  A message type containing a dozen interfaces is a bit annoying to untangle if you need to delve deep into message routing to troubleshoot an issue.

> [!CAUTION]
> * Class inheritance has the same guidance as interfaces, but with more caution.
> * Consuming a base class type, and expecting polymorphic method behavior almost always leads to problems.
> * A big base class may cause pain down the road as changes are made, particularly when supporting multiple message versions.

---

### Sharing Messages Between Services

<br/>

> [!IMPORTANT]
> When two applications or services exchange the same message(for example, when one application publishes a message and another consumes it) OpenTransit has a single strict requirement for correct operation: 
the message must use **the same namespace** in both systems.

OpenTransit uses the combination of **message name** and **namespace** to define the broker topology, which is why this requirement is enforced.

OpenTransit does not require the publisher and subscriber to share a NuGet package, a common project reference, or any shared code. Message contracts may be defined independently in each application, provided the same message uses the same namespace in both contexts.

However, it is recommended to keep the Message Contracts in a class Library and share them between the applications via Nuget Reference(especially if you need Versioning). 

---


