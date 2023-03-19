# Microservices

## What are microservices

Microservices - also known as the microservice architecture - is an architectural style that structures an application as a collection of services that are

- Independently deployable
- Loosely coupled
- Organized around business capabilities
- Owned by a small team
- Highly maintainable and testable
The microservice architecture enables the rapid, frequent and reliable delivery of large, complex applications. It also enables an organization to evolve its technology stack.

## Comparing Architectures

You are developing a server-side enterprise application. It must support a variety of different clients including desktop browsers, mobile browsers and native mobile applications. The application might also expose an API for 3rd parties to consume. It might also integrate with other applications via either web services or a message broker. The application handles requests (HTTP requests and messages) by executing business logic; accessing a database; exchanging messages with other systems; and returning a HTML/JSON/XML response. There are logical components corresponding to different functional areas of the application.

### Problem

What’s the application’s deployment architecture?

### Forces

- There is a team of developers working on the application
- New team members must quickly become productive
- The application must be easy to understand and modify
- You want to practice continuous deployment of the application
- You must run multiple instances of the application on multiple machines in order to satisfy scalability and availability requirements
- You want to take advantage of emerging technologies (frameworks, programming languages, etc)

### Monolithic Architecture

Let’s imagine that you are building an e-commerce application that takes orders from customers, verifies inventory and available credit, and ships them. The application consists of several components including the StoreFrontUI, which implements the user interface, along with some backend services for checking credit, maintaining inventory and shipping orders. The application is deployed as a single monolithic application.

![monoArquitecture](https://microservices.io/i/DecomposingApplications.011.jpg)

#### Benefits

- Simple to develop: the goal of current development tools and IDEs is to support the development of monolithic applications
- Simple to deploy: you simply need to deploy the WAR file (or directory hierarchy) on the appropriate runtime
- Simple to scale: you can scale the application by running multiple copies of the application behind a load balancer

#### Drawbacks

- The large monolithic code base intimidates developers, especially ones who are new to the team. The application can be difficult to understand and modify.
- Also, because there are not hard module boundaries, modularity breaks down over time. Moreover, because it can be difficult to understand how to correctly implement a change the quality of the code declines over time. It’s a downwards spiral.
- Overloaded IDE - the larger the code base the slower the IDE and the less productive developers are.
- Overloaded web container - the larger the application the longer it takes to start up. This had have a huge impact on developer productivity because of time wasted waiting for the container to start. It also impacts deployment too.
- Scaling the application can be difficult -This architecture can’t scale with an increasing data volume. Each copy of application instance will access all of the data, which makes caching less effective and increases memory consumption and I/O traffic. Also, different application components have different resource requirements - one might be CPU intensive while another might memory intensive. With a monolithic architecture we cannot scale each component independently
- Obstacle to scaling development -  Once the application gets to a certain size its useful to divide up the engineering organization into teams that focus on specific functional areas.  Once the application gets to a certain size its useful to divide up the engineering organization into teams that focus on specific functional areas.
- Requires a long-term commitment to a technology stack - a monolithic architecture forces you to be married to the technology stack (and in some cases, to a particular version of that technology) you chose at the start of development . With a monolithic application, can be difficult to incrementally adopt a newer technology.

### Microservice Architecture

Let’s imagine that you are building an e-commerce application that takes orders from customers, verifies inventory and available credit, and ships them. The application consists of several components including the StoreFrontUI, which implements the user interface, along with some backend services for checking credit, maintaining inventory and shipping orders. The application consists of a set of services.

![microservice](https://microservices.io/i/Microservice_Architecture.png)

#### Benefits

- Enables the continuous delivery and deployment of large, complex applications.
  - Improved maintainability - each service is relatively small and so is easier to understand and change
  - Better testability - services are smaller and faster to test
  - Better deployability - services can be deployed independently
  - It enables you to organize the development effort around multiple, autonomous teams. Each (so called two pizza) team owns and is responsible for one or more services. Each team can develop, test, deploy and scale their services independently of all of the other teams.
-Each microservice is relatively small:
  - Easier for a developer to understand
  - The IDE is faster making developers more productive
  - The application starts faster, which makes developers more     - productive, and speeds up deployments
- Improved fault isolation. For example, if there is a memory leak in one service then only that service will be affected.
- Eliminates any long-term commitment to a technology stack. When developing a new service you can pick a new technology stack. Similarly, when making major changes to an existing service you can rewrite it using a new technology stack.

#### Drawbacks

- Developers must deal with the additional complexity of creating a distributed system:
  - Developers must implement the inter-service communication mechanism and deal with partial failure
  - Implementing requests that span multiple services is more difficult
  - Testing the interactions between services is more difficult
  - Implementing requests that span multiple services requires careful coordination between the teams
  - Developer tools/IDEs are oriented on building monolithic applications and don’t provide explicit support for developing distributed applications.

#### Issues

One challenge with using this approach is deciding when it makes sense to use it. When developing the first version of an application, you often do not have the problems that this approach solves. Moreover, using an elaborate, distributed architecture will slow down development. This can be a major problem for startups whose biggest challenge is often how to rapidly evolve the business model and accompanying application.

Other issues that arise with microservices are:

- How to decompose the application into services? (Decompose patterns)
- How to maintain data consistency? (Saga pattern, event sourcing etc)
- How to implement queries? (API Composition and Command Query Responsibility Segregation (CQRS) patterns.) 

![micro patterns](https://microservices.io/i/PatternsRelatedToMicroservices.jpg)

## Decomposition

### Decompose by business capability

Business capabilities are often organized into a multi-level hierarchy. For example, an enterprise application might have top-level categories such as Product/Service development, Product/Service delivery, Demand generation, etc.

Identifying business capabilities and hence services requires an understanding of the business. An organization’s business capabilities are identified by analyzing the organization’s purpose, structure, business processes, and areas of expertise. Bounded contexts are best identified using an iterative process. Good starting points for identifying business capabilities are:

- organization structure - different groups within an organization might correspond to business capabilities or business capability groups.
- high-level domain model - business capabilities often correspond to domain objects

#### Example

The business capabilities of an online store include:

- Product catalog management
- Inventory management
- Order management
- Delivery management

The corresponding microservice architecture would have services corresponding to each of these capabilities.

![by capabilities](https://microservices.io/i/decompose-by-business-capability.png)

### Decompose by subdomain

Define services corresponding to Domain-Driven Design (DDD) subdomains. DDD refers to the application’s problem space - the business - as the domain. A domain is consists of multiple subdomains. Each subdomain corresponds to a different part of the business.

Subdomains can be classified as follows:

- Core - key differentiator for the business and the most valuable part of the application
- Supporting - related to what the business does but not a differentiator. These can be implemented in-house or outsourced.
- Generic - not specific to the business and are ideally implemented using off the shelf software

This pattern has the following benefits:

- Stable architecture since the subdomains are relatively stable
- Development teams are cross-functional, autonomous, and organized around delivering business value rather than technical features
- Services are cohesive and loosely coupled

There are the following issues to address:

- How to identify the subdomains? Identifying subdomains and hence services requires an understanding of the business. Like business capabilities, subdomains are identified by analyzing the business and its organizational structure and identifying the different areas of expertise. Subdomains are best identified using an iterative process. Good starting points for identifying subdomains are:

- organization structure - different groups within an organization might correspond to subdomains
- high-level domain model - subdomains often have a key domain object

#### Example

![by subdomain](https://microservices.io/i/decompose-by-subdomain.png)

### Self-contained service

Consider, the FTGO application, which is an online food delivery application. A client of application creates an order by making an HTTP POST /orders request and expects a response, say, within 600ms. Because the FTGO application uses the microservice architecture, the responsibilities that implement order creation are scattered across multiple services. The POST request is first routed to the Order Service, which must then collaborate with the following services:
  
- Restaurant Service - knows the restaurant’s menu and prices
- Consumer Service - knows the state of the Consumer that places the order
- Kitchen Service - creates a Ticket, which tells the chef what to cook
- Accounting Service - authorizes the consumer’s credit card
The Order Service could invoke each of these services using synchronous request/response. It might, for example, implement the inter-service communication using REST or gRPC.

![synchronus service](https://microservices.io/i/Order-Service-synchronous.png)

However, a key drawback of using synchronous request/response is that it reduces availability. That’s because if any of the Order Sevice’s collaborators are unavailable, it will not be able to create the order and must return an error to the client.

An alternative approach is to eliminate all synchronous communication between the Order Service and its collaborators by using the CQRS and Saga patterns. The Order Service can use the CQRS pattern to maintain a replica of the restaurant menu’s and there by eliminate the need to synchronously fetch data from the Restaurant Service. It can validate the order asynchronously by using the Saga pattern. The Order Service creates an Order in a PENDING state and sends back a response to the POST /order. It then completes the creation of the order by communicating asynchronously with the other services.

#### Solution

![self contained](https://microservices.io/i/SelfContainedService.png)
One way to make a service self-contained is to implement needed functionality as a service module rather than a separate service. We could, for example, merge the Order Service and Restaurant Service.

Another way to make a service self-contained is for it to collaborate with other services using the CQRS and the Saga patterns. A self-contained service uses the Saga pattern to asynchronously maintain data consistency. It uses the CQRS pattern to maintain a replica of data owned by other services. The Order Service in the FTGO application described earlier is an example of a self-contained service. The `createOrder()` operation, for example, queries a CQRS replica of data owned by the Restaurant Service to validate and price the order, and then initiates a saga to finish the creation of the order.

This pattern has the following benefits:

- Improved availability and response time

This pattern has the following drawbacks:

- Increased cost and complexity of using CQRS
- Increased complexity of using sagas
- Less straightforward API when using sagas
- Larger service due to functionality being implemented in the service instead of as a separate service

### Service per team

One approach is a shared ownership model where multiple teams to work on each service as necessary. For example, each team might be responsible for implementing features that span multiple services. On the one hand, this approach aligns teams with the user experience. But on the other hand, it increases the amount of coordination needed between the teams. Also, the lack of code ownership increases the risk of poor code quality.

A better approach, which increases team autonomy and loose coupling, is a code/service ownership model. The team, which is responsible for a business function/capability owns a code base, which they deploy as one of more services. As a result, the team can freely develop, test, deploy and scale its services. They primarily interact with other teams in order to negotiate APIs.

A team should ideally own just one service since that’s sufficient to ensure team autonomy and loose coupling and each additional service adds complexity and overhead. A team should only deploy its code as multiple services if it solves a tangible problem, such as significantly reducing lead time or improving scalability or fault tolerance.

Since a team must be small, its cognitive capacity is limited. In order for the team to be productive, its code base should be scoped to not exceed the team’s cognitive capacity. In other words, it must ‘fit’ in the team’s heads. As a result, there is an upper bound on the size and/or complexity of a service.

![per team](https://microservices.io/i/ServicePerTeam.png);

This pattern has the following benefits:

- Enables each team to be autonomous and work with minimal coordination with other teams
- Enables the teams to be loosely coupled
- Achieves team autonomy and loose coupling with the minimum number of services
- Improves code quality due to long term code ownership

This pattern has the following drawbacks:

- Teams are not necessarily aligned with end user features
- Implementing features that span services is more complicated and requires teams to collaborate

## Refactoring

### Strangler application

How do you migrate a legacy monolithic application to a microservice architecture? Modernize an application by incrementally developing a new (strangler) application around the legacy application. In this scenario, the strangler application has a microservice architecture.

![Strangler application](https://microservices.io/i/decompose-your-monolith-devnexus-feb-2020.001.jpeg)

### Anti-corruption Layer pattern

Implement a façade or adapter layer between different subsystems that don't share the same semantics. This layer translates requests that one subsystem makes to the other subsystem. Use this pattern to ensure that an application's design is not limited by dependencies on outside subsystems.

Often legacy systems suffer from quality issues such as convoluted data schemas or obsolete APIs. The features and technologies used in legacy systems can vary widely from more modern systems. To interoperate with the legacy system, the new application may need to support outdated infrastructure, protocols, data models, APIs, or other features that you wouldn't otherwise put into a modern application.

Isolate the different subsystems by placing an anti-corruption layer between them. This layer translates communications between the two systems, allowing one system to remain unchanged while the other can avoid compromising its design and technological approach.

![anti corruption](https://learn.microsoft.com/en-us/azure/architecture/patterns/_images/anti-corruption-layer.png)

#### Issues and considertation

- The anti-corruption layer may add latency to calls made between the two systems.
- The anti-corruption layer adds an additional service that must be managed and maintained.
- Consider how your anti-corruption layer will scale.
- Consider whether you need more than one anti-corruption layer. You may want to decompose functionality into multiple services using different technologies or languages, or there may be other reasons to partition the anti-corruption layer.
- Consider how the anti-corruption layer will be managed in relation with your other applications or services. How will it be integrated into your monitoring, release, and configuration processes?
- Make sure transaction and data consistency are maintained and can be monitored.
- Consider whether the anti-corruption layer needs to handle all communication between different subsystems, or just a subset of features.
- If the anti-corruption layer is part of an application migration strategy, consider whether it will be permanent, or will be retired after all legacy functionality has been migrated.
- This pattern is illustrated with distinct subsystems above, but can apply to other service architectures as well, such as when integrating legacy code together in a monolithic architecture.

#### When to use this pattern

Use this pattern when:

- A migration is planned to happen over multiple stages, but integration between new and legacy systems needs to be maintained.
- Two or more subsystems have different semantics, but still need to communicate.

This pattern may not be suitable if there are no significant semantic differences between new and legacy systems.

## Data Management

### Databse per service

Let’s imagine you are developing an online store application using the Microservice architecture pattern. Most services need to persist data in some kind of database. For example, the Order Service stores information about orders and the Customer Service stores information about customers.

Keep each microservice’s persistent data private to that service and accessible only via its API. A service’s transactions only involve its database.

The following diagram shows the structure of this pattern.

![microservices](https://microservices.io/i/databaseperservice.png)

There are a few different ways to keep a service’s persistent data private. You do not need to provision a database server for each service. For example, if you are using a relational database then the options are:

- Private-tables-per-service – each service owns a set of tables that must only be accessed by that service
- Schema-per-service – each service has a database schema that’s private to that service
- Database-server-per-service – each service has it’s own database server.
  
Private-tables-per-service and schema-per-service have the lowest overhead. Using a schema per service is appealing since it makes ownership clearer. Some high throughput services might need their own database server.

It is a good idea to create barriers that enforce this modularity. You could, for example, assign a different database user id to each service and use a database access control mechanism such as grants. Without some kind of barrier to enforce encapsulation, developers will always be tempted to bypass a service’s API and access it’s data directly.

Using a database per service has the following benefits:

- Helps ensure that the services are loosely coupled. Changes to one service’s database does not impact any other services.

- Each service can use the type of database that is best suited to its needs. For example, a service that does text searches could use ElasticSearch. A service that manipulates a social graph could use Neo4j.

Using a database per service has the following drawbacks:

- Implementing business transactions that span multiple services is not straightforward. Distributed transactions are best avoided because of the CAP theorem. Moreover, many modern (NoSQL) databases don’t support them.
- Implementing queries that join data that is now in multiple databases is challenging.
- Complexity of managing multiple SQL and NoSQL databases

There are various patterns/solutions for implementing transactions and queries that span services:

Implementing transactions that span services - use the Saga pattern.

Implementing queries that span services

### Shared Database (it's almost an antipattern)

Use a (single) database that is shared by multiple services. Each service freely accesses data owned by other services using local ACID transactions.

The benefits of this pattern are:
  
- A developer uses familiar and straightforward ACID transactions to enforce data consistency
- A single database is simpler to operate

The drawbacks of this pattern are:

- Development time coupling - a developer working on, for example, the OrderService will need to coordinate schema changes with the developers of other services that access the same tables. This coupling and additional coordination will slow down development.
- Runtime coupling - because all services access the same database they can potentially interfere with one another. For example, if long running CustomerService transaction holds a lock on the ORDER table then the OrderService will be blocked.
- Single database might not satisfy the data storage and access requirements of all services.

### Saga Pattern

The Saga design pattern is a way to manage data consistency across microservices in distributed transaction scenarios. A saga is a sequence of transactions that updates each service and publishes a message or event to trigger the next transaction step. If a step fails, the saga executes compensating transactions that counteract the preceding transactions.

[Saga by Microsoft](https://learn.microsoft.com/en-us/azure/architecture/reference-architectures/saga/saga)

### API Gateways

In a microservices architecture, a client might interact with more than one front-end service. Given this fact, how does a client know what endpoints to call? What happens when new services are introduced, or existing services are refactored? How do services handle SSL termination, authentication, and other concerns? An API gateway can help to address these challenges.

![API gateway](https://learn.microsoft.com/en-us/azure/architecture/microservices/images/gateway.png)

[API gateway by microsoft](https://learn.microsoft.com/en-us/azure/architecture/microservices/images/gateway.png)

### Command Query Responsibility Segregation (CQRS)

A pattern that separates read and update operations for a data store. Implementing CQRS in your application can maximize its performance, scalability, and security. The flexibility created by migrating to CQRS allows a system to better evolve over time and prevents update commands from causing merge conflicts at the domain level.

[CQRS](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs#context-and-problem)

### Event Sourcing

Instead of storing just the current state of the data in a domain, use an append-only store to record the full series of actions taken on that data. The store acts as the system of record and can be used to materialize the domain objects. This can simplify tasks in complex domains, by avoiding the need to synchronize the data model and the business domain, while improving performance, scalability, and responsiveness. It can also provide consistency for transactional data, and maintain full audit trails and history that can enable compensating actions.

[Event Sourcing pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs#context-and-problem)
