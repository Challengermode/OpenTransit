## Documentation as a First-Class Citizen

At OpenTransit, documentation is a **first-class citizen**. We believe that great software is only as good as its ability to be understood, adopted, and extended. 

That’s why we treat documentation with the same level of importance as the project’s source code — clear, accurate, and helpful documentation is a core part of our mission.


## Our Current Focus: Understanding Through Clarity

MassTransit v8 will continue receiving support until the end of 2026. During this period, our primary focus is to build an outstanding documentation experience for OpenTransit.  
We are actively researching the codebase, exploring real-world use cases, and developing analogies, explanations, and examples that make distributed system concepts easier to understand.  

For example, As part of this effort, we have introduced a concept called the **[Generic Broker](concepts/generic-broker.md)** — an abstraction that captures the common functionalities shared by all message brokers.  
For each [pattern](why-opentransit#patterns) and other concepts, we will explain the underlying [topology](concepts/topology) using the [Generic Broker](concepts/generic-broker.md) first, and then show how to customize it for each broker’s specific implementation.

Initially, the examples, will be given with **RabbitMQ**, and **Mongodb**. however, we will cover other broker's and db's soon. 


Our goal is to create **Understanding Documentation** — content that not only tells you *how* to use the framework, but also helps you truly *grasp* the reasoning and patterns behind it.

## Community Feedback Matters

Because documentation is a first-class citizen, we strongly encourage the community to participate.  
If any part of the documentation feels unclear, incomplete, confusing, or could simply be improved — **please let us know**.

We welcome:

- Feedback  
- Suggestions  
- Requests for clarification  
- Reports of unclear or hard-to-follow sections  

Your input directly helps us improve OpenTransit for everyone. We’re fully open to community collaboration and want to build documentation that genuinely serves developers at all levels.
