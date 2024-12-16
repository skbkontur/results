This document establishes a set of best practices related to development of clients for HTTP microservices using Vostok libraries. 

An example of a client that implements these practices is [HerculesTimelineClient](https://github.com/vostok/hercules.client/blob/master/Vostok.Hercules.Client/HerculesTimelineClient.cs).

### General

- _Always_ target `netstandard2.0` to enable both .NET Framework and .NET Core users to make use of the library. 
- _Always_ make client methods asynchronous.
- _Always_ make client instances thread-safe.
- _Always_ create a client interface (even if it will only ever have one implemenation) to enable unit-testing against it.
- _Always_ incapsulate serialization and deserialization of models in the client code.
- _Consider_ decorating client methods and models with JetBrains annotations (`NotNull`, `CanBeNull`, etc).
- _Consider_ documenting client methods and models with xml-docs in code.
  - These docs should generally help users understand client's guarantees and behaviour.

<br/>

### Method parameters

- _Always_ add a timeout parameter of `TimeSpan` type to client methods.
- _Always_ add a `CancellationToken` parameter to client methods.
- _Consider_ passing all the other parameters to client methods as request/query objects with properties instead of passing them directly as method arguments.
  - This will help to avoid breaking backward compatibility later and keep the interfaces clean.
    ```
    public class ReadQuery 
    {
      // Extensible without breaking backward compatibility.
      // Required properties are passed to constructor.
      // Optional properties can just have default values.
    }
    ```
    
<br/>

### Method results

- _Consider_ using result objects with status enums to present a user-friendly mechanism for error handling. 
  - A common pattern to build such a result object is to include a status (`success`/`timeout`/etc.) and an optional payload (service response in case of success).
  - Users are therefore able to distinguish and handle different error conditions without relying on catching exceptions.
  - Unconditionally accessing the payload of a result object that represents failure should throw an exception.

<br/>

### Method extensions

- _Always_ create synchronous extensions for all asynchronous methods using `.GetAwaiter().GetResult()`.

<br/>

### ClusterClient

- _Always_ use [ClusterClient](https://www.nuget.org/packages?q=vostok.clusterclient) library to send HTTP requests.
- _Always_ use universal [transport](https://www.nuget.org/packages/Vostok.ClusterClient.Transport/) implementation to perform equally well on every runtime.
- _Always_ set target service name in client configuration.
- _Consider_ using `ForkingRequestStrategy` to tolerate slow replicas.
- _Consider_ enabling replica budgeting and adaptive throttling to protect from overload.

<br/>

### Configuration

- _Always_ expose an optional [ClusterClientSetup](https://github.com/vostok/clusterclient.core/blob/master/Vostok.ClusterClient.Core/ClusterClientSetup.cs) property that can be used to customize internal client instances.
- _Always_ allow to pass an external [IClusterProvider](https://github.com/vostok/clusterclient.core/blob/master/Vostok.ClusterClient.Core/Topology/IClusterProvider.cs)  or `Func<Uri[]>` instance to set service topology.
- _Consider_ including a default topology source _if_ it suits the majority of client library's users. 
- _Consider_ making settings dynamic (through `Func<T>`) where applicable.

<br/>

### Logging
- _Always_ provide an option to pass an external `ILog` instance.
- _Consider_ using [LogProvider](https://github.com/vostok/logging.abstractions/blob/master/Vostok.Logging.Abstractions/LogProvider.cs) when user-provided log is `null`.
- _Consider_ adding log source context with `log = log.ForContext<MyClient>();` call.

<br/>

### Tracing
- _Always_ enable tracing for ClusterClient with `vostok.clusterclient.tracing` module.
- _Always_ provide an option to pass an external `ITracer` instance.
- _Consider_ using [TracerProvider](https://github.com/vostok/tracing.abstractions/blob/master/Vostok.Tracing.Abstractions/TracerProvider.cs) when user-provided tracer is `null`.
