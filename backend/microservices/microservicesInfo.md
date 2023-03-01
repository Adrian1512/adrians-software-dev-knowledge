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