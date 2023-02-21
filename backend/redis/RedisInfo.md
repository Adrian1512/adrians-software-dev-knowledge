# Redis

## What is Redis?

Redis is an open source, in memory, key-value data store most commonly used as a primary database, cache, message brokerm and queque. It's usually used as caching, session management, gaming, fraud detection, leaderboards, real-time analytics, geospatial indexing, ride-sharing, social media, and streaming applications.

## Benefits 🤑

### Performance

The primary benefit of Redis is its sub-millisecond queries. Redis runs in-memory, which enables low-latency and high throughput. Running in-memory means requests for data do not require a trip to disk. This leads to an order of magnitude more operations and faster response times. Redis is one of the only databases that supports millions of operations per second.

### Data structures

- Strings - any text or binary data (512MB max.)
- Hashes - field-value pairs that most commonly represent objects
- Lists - a collection of Strings ordered by when they were added as a linked list. Useful for queues and "latest updates" for social media posts
- Sets - an unordered collection of Strings with the ability to intersect, union, and diff against other Sets
- Sorted Sets - similar to a Redis Set, the Sorted Set is a collection of unique String members. In a Sorted Set, each member is associated with a score that can be used to sort the collection.
- Bitmaps - not necessarily a data type, but a set of bit-oriented operations on the String type
- HyperLogLogs - a probabilistic data structure used in order to count unique things (cardinality of a set)
- Geospatial - a Sorted Set of longitude/latitude/name key-value pairs useful for maps, geosearching, and "nearby" features
- Streams - a data type that models an append only log and which can be used as a durable message queue

### Modules

- RediSearch - a real-time search and secondary indexing engine that runs on your Redis dataset and allows you to query data that has just been indexed
- RedisJSON - a native JSON data type tailored for fast, efficient, in-memory storage and retrieval of JSON documents at high speed and volume
- RedisGears - a programmable engine for Redis that runs inside Redis, closer to where your data lives, and which allows cluster-wide operations across shards, nodes, data structures, and data models at a sub-millisecond speed
- RedisAI - a machine learning data type that runs inside Redis and allows you to train and predict on your data. Additionally provides a common layer among different formats and platforms, including PyTorch, TensorFlow/TensorRT, and ONNX Runtime
- RedisGraph - a graph data structure that can be used to store and query data in a graph-oriented way. Supports the industry-standard Cypher as a query language and incorporates the state-of-the-art SuiteSparse GraphBLAS engine for matrix operations on sparse matrices
- RedisTimeSeries - a time series data type with capabilities like automatic downsampling, aggregations, labeling and search, compression, and enhanced multi-range queries as well as built-in connectors to popular monitoring tools like Prometheus and Grafana to enable the extraction of data into useful formats for visualization and monitoring
- RedisBloom - provides Redis with support for additional probabilistic data structures and allows for constant memory space and extremely fast processing while still maintaining a low error rate. Supports Bloom and Cuckoo filters to determine whether an item is present or absent from a collection with a given degree of certainty, Count-min sketch to count the frequency of the different items in sub-linear space, and Top-K to count top k events in a near deterministic manner

### Replica and persistence

Redis offers asynchronous replication where data can be replicated to multiple servers. This allows for improved read performance and faster recovery

### High availability and scalability

Redis provides a primary-replica architecture as a single node or cluster.

### Redis vs. Memcached

Both Redis and Memcached are open source, powerful, in-memory data stores. The main difference between the two is that Redis is a more full-featured database that is built to fit a number of different use cases. Memcached is primarily used for key/value caching. Redis is used for both caching and as a database.

## RedisInsight 📱

It is a fully-featured pure Desktop GUI client that provides capabilities to design, develop and optimize your Redis application. It works with any cloud provider as long as you run it on a host with network access to your cloud-based Redis server.

## RedisMod 🐋

This simple container image bundles together the latest stable releases of Redis and select Redis modules. This image is based on the official image of Redis from Docker. By default, the container starts with Redis' default configuration and all included modules loaded.

Modules included in the container

- RediSearch: a full-featured search engine
- RedisGraph: a graph database
- RedisTimeSeries: a timeseries database
- RedisAI: a tensor and deep learning model server
- RedisJSON: a native JSON data type
- RedisBloom: native Bloom and Cuckoo Filter data types
- RedisGears: a dynamic execution framework

## Continuous Deployment ♻️

Main available options are:

- [Jenkins](https://developer.redis.com/operate/continuous-integration-continuous-deployment/jenkins)
- [CircleCI](https://developer.redis.com/operate/continuous-integration-continuous-deployment/circleci)
- [Argo CD](https://developer.redis.com/operate/continuous-integration-continuous-deployment/argocd)

## Observabilty 👀

Want to see how your data is behaving live here are some options:

- [Grafana](https://developer.redis.com/operate/observability/redisdatasource)
- [Datadog](https://developer.redis.com/operate/observability/datadog)

## Anti-Patters ⛔

### 1. Large databases running on a single shard/Redis instance

A general rule of thumb is 25GB or 25K Ops/Second. If you have more than that, please do sharding otherwise, fail over, backup and recovery will take longer, meaning more downtime

### 2.  One connection per request

While opening and closing connections per command will technically work, it’s far from optimal and needlessly cuts into the performance of Redis as a whole. Hence, it is recommended to use a connection pool (Jedis, a java connection pool) or a Redis client that has a reactive design (Lettuce).

Using the OSS Cluster API, the connection to the nodes are maintained by the client as needed, so you’ll have multiple connections open to different nodes at any given time. With Redis Enterprise, the connection is actually to a proxy, which takes care of the complexity of connections at the cluster level.

**Conection pooling**
![image.jpg](https://i.stack.imgur.com/9GeMX.jpg)

### 3. Connecting directly to Redis instances

With a large number of clients, a reconnect flood will be able to simply overwhelm a single threaded Redis process and force a failover. Hence, it is recommended that you should use the right tool that allows you to reduce the number of open connections to your Redis server.

**Redis Enterprise DMC** proxy allows you to reduce the number of connections to your cache server by acting as a proxy. There are other 3rd party tool like **Twemproxy**. It is a fast and lightweight proxy server that allows you to reduce the number of open connections to your Redis server. It was built primarily to reduce the number of connections to the caching servers on the backend. This, together with protocol pipelining and sharding enables you to horizontally scale your distributed caching architecture.

**Protocol pipelining:** is a technique in which multiple requests are written out to a single socket without waiting for the corresponding responses.
![protocol-pipelining.png](https://upload.wikimedia.org/wikipedia/commons/thumb/1/19/HTTP_pipelining2.svg/1280px-HTTP_pipelining2.svg.png)

### 4. More than one secondary shard (Redis OSS)

Redis OSS uses a shard-based quorum. It's advised to use at least 3 copies of the data (2 replica shards per master shard) in order to be protected from split-brain situations. In nutshell, Redis OSS solves the quorum challenge by having an odd number of shards (primary + 2 replicas).
