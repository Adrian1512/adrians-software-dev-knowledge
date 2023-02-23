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

**Split brain:** In a clustering context, is a state in which a cluster of nodes gets divided (or partitioned) into smaller clusters of equal numbers of nodes, each of which believes it is the only active cluster.

Believing the other clusters are dead, each cluster may simultaneously access the same application data or disks, which can lead to data corruption. A split brain situation is created during cluster reformation.

### 5. Performing single operation

Performing several operations serially increases connection overhead. Instead, use Redis Pipelining. Pipelining is the process of sending multiple messages down the pipe without waiting on the reply from each - and (typically) processing the replies later when they come in.

Pipelining is completely a client side implementation. It is aimed at solving response latency issues in high network latency environments. So, the lesser the amount of time spent over the network in sending commands and reading responses, the better. This is effectively achieved by buffering. The client may (or may not) buffer the commands at the TCP stack (as mentioned in other answers) before they are sent to the server. Once they are sent to the server, the server executes them and buffers them on the server side. The benefit of the pipelining is a drastically improved protocol performance. The speedup gained by pipelining ranges from a factor of five for connections to localhost up to a factor of at least one hundred over slower internet connections.

### 6. Caching keys without TTL

Redis functions primarily as a key-value store. It is possible to set timeout values on these keys. Said that, a timeout expiration automatically deletes the key. Additionally, when we use commands that delete or overwrite the contents of the key, it will clear the timeout. Redis TTL command is used to get the remaining time of the key expiry in seconds. TTL returns the remaining time to live of a key that has a timeout. This introspection capability allows a Redis client to check how many seconds a given key will continue to be part of the dataset.Keys will accumulate and end up being evicted. Hence, it is recommended to set TTLs on all caching keys.

### 7. Endless Redis Replication Loop

When attempting to replicate a very large active database over a slow or saturated link, replication never finishes due to the continuous updates. Hence, it is recommended to tune the slave and client buffers to allow for slower replication.

### 8. Hot keys

if you centralize the access down to a few pieces of data accessed constantly, you create what is known as a hot-key problem.  The data is stored in one single, primary location based off of hashing that key. So, when you access a single key over and over again, you’re actually accessing a single node/shard over and over again. Let’s put it another way—if you have a cluster of 99 nodes and you have a single key that gets a million requests in a second, all million of those requests will be going to a single node, not spread across the other 98 nodes.

 The best defence is to avoid the development pattern that is creating the situation. Writing the data to multiple keys that reside in different shards will allow you to access the same data more frequently. In nutshell, having specific keys that are accessed with every client operation. Hence, it's recommended to shard out hot keys using hashing algorithms. You can set policy to LFU and run redis-cli --hotkeys to determine.

### 9. Using keys command

In Redis, the KEYS command can be used to perform exhaustive pattern matching on all stored keys. This is not advisable, as running this on an instance with a large number of keys could take a long time to complete, and will slow down the Redis instance in the process. In the relational world, this is equivalent to running an unbound query (SELECT...FROM without a WHERE clause). Execute this type of operation with care, and take necessary measures to ensure that your tenants are not performing a KEYS operation from within their application code. Use SCAN, which spreads the iteration over many calls, not tying up your whole server at one time.

Scaning keyspace by keyname is an extremely slow operation and will run O(N) with N being the number of keys. It is recommended to use RediSearch to return information based on the contents of the data instead of iterating through the key space.

### 10. Running Ephemeral Redis as a primary database

 Using Redis as a primary database requires two extra features to be effective. Any primary database should really be highly available. If a cache goes down, then generally your application is in a brown-out state. If a primary database goes down, your application also goes down. Similarly, if a cache goes down and you restart it empty, that’s no big deal. For a primary database, though, that’s a huge deal. Redis can handle these situations easily, but they generally require a different configuration than running as a cache. Redis as a primary database is great, but you’ve got to support it by turning on the right features.

With Redis open source, you need to set up Redis Sentinel for high availability. In Redis Enterprise, it’s a core feature that you just need to turn on when creating the database. As for durability, both Redis Enterprise and open source Redis provide durability through AOF or snapshotting so your instance(s) start back up the way you left them.

### 11.  Storing JSON blobs in a string

Microservices written in several languages may not marshal/unmarshal JSON in a consistent manner. Application logic will be required to lock/watch a key for atomic updates. JSON manipulation is often a very compute costly operation. Hence, it is recommended to use HASH data structure and also, RedisJSON module.

### 12. Translating a table or JSON to a HASH without considering query pattern

The only query mechanism is a SCAN which requires reading the data structure and limits filtering to the MATCH directive. It is recommended to store the table or JSON as a string. Break out the indexes into reverse indexes using a SET or SORTED SET and point back to the key for the string. Using SELECT command and multiple databases inside one Redis instance

The usage of SELECT and multiple databases inside one Redis instance was mentioned as an anti-pattern by Salvatore (the creator of Redis). It is recommended to use a dedicated Redis instance for each database need. This is especially true in microservice architectures where client applications might step on each other's toes (noisy neighbor, database setup/teardown impact, maintenance, upgrade, ...)

The RedisTimeSeries module provides a direct compete to time series databases. But if the only query is based on ordering, it's unnecessary complexity. Hence, it is recommended to use a SORTED SET with a score of 0 for every value. The values are appended. Or use a timestamp for the score for simple time based queries.

