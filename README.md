# Implementation of Dapr Actor multi-tenancy

This is an example project containing a custom implementation that supports dapr actor multi-tenancy, where one instance of the state store (Redis, MongoDB, SQLite) is used and there are many instances of the same application, that uses actors, running in the same environment and accessing the same instance of the state store.

Since actor state keys are not prefixed by the store component, the idea is to create a unique namespace for each application instance (could be a kubernetes namespace for example) and when registering the actor types prefix prefix each type with the application's namespace. This will mean no actors with the same type are registered and therefore no conflicts in the actor state keys are possible.

`./DaprNamespace/DaprBaseExtensions.cs` is the most important implementation details that exposes `AddActorsWithNamespace` method which must be used for registering all actors in the application. The method registers a custom implementation of `IActorProxyFactory`, `NamespaceActorProxyFactory`, that adds the namespace configuration for actor type names when creating actor proxies. Thus `AddActorsWithNamespace` and `NamespaceActorProxyFactory` work together making sure both registering and creating actors takes into account the application's namespace. The downside of this approach is that you have to **remember** to use the extension method to register the actor types and remember to inject `IActorProxyFactory` everywhere where actors are created, instead of using the static methods on `ActorProxy`.

The namespace can be set via en environment variable or any other standard method defined for configuration in dotnet applications.

## Run the demo

To run the dapr side cars, run `./run-dapr.sh`. To run two instances of the app with different namespaces:

```sh
Dapr__Namespace=test-1 DAPR_HTTP_PORT=3501 DAPR_GRPC_PORT=54600 dotnet run --urls=http://localhost:5011/
Dapr__Namespace=test-2 DAPR_HTTP_PORT=3502 DAPR_GRPC_PORT=54601 dotnet run --urls=http://localhost:5013/
```

Navigate to `http://localhost:5011/swagger/index.html` and `http://localhost:5013/swagger/index.html` and set some state in API, then connect to the redis instance running in `docker` via `docker exec -it <CONTAINER_ID> /bin/sh` and run the following commands to see generated state keys:

```sh
redis-cli
keys *
```
