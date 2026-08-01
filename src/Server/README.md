# Open Service Broker API for .NET - Server

Server library implementing the [Open Service Broker API](https://www.openservicebrokerapi.org/) specification using ASP.NET Core. This specification allows developers, ISVs, and SaaS vendors a single, simple, and elegant way to deliver services to applications running within cloud native platforms such as Cloud Foundry, OpenShift, and Kubernetes.

    dotnet add package OpenServiceBroker.Server

## Usage

Set up a regular ASP.NET Core 8.0+ project. Then implement the following interfaces:
- `ICatalogService` (optionally also `IETagProvider` and/or `ILastModifiedProvider` on the same class)
- either `IServiceInstanceBlocking` or `IServiceInstanceDeferred` or both
- either `IServiceBindingBlocking` or `IServiceBindingDeferred` or both

Register your implementations in the `IServiceCollection` for dependency injection. For example:

```csharp
services.AddTransient<ICatalogService, MyCatalogService>()
        .AddTransient<IServiceInstanceBlocking, MyServiceInstanceBlocking>()
        .AddTransient<IServiceBindingBlocking, MyServiceBindingBlocking>();
```

Then enable MVC Controllers using `.AddMvc()` or `.AddControllers()` followed by calling the `.AddOpenServiceBroker()` extension method:

```csharp
services.AddControllers()
        .AddOpenServiceBroker();
```

You can use the [project template](https://github.com/TypedRest/OpenServiceBroker/tree/master/template) to quickly set up a pre-configured ASP.NET Core 8.0 project with `OpenServiceBroker.Server`.

### Versioning

The Server Library inspects the `X-Broker-API-Version` header for all requests (as defined in the specification). Currently it accepts all versions from `2.0` to `2.16`.

## Related packages

- [OpenServiceBroker.Model](https://www.nuget.org/packages/OpenServiceBroker.Model/) provides the DTOs and exceptions used by this library.
- [OpenServiceBroker.Client](https://www.nuget.org/packages/OpenServiceBroker.Client/) is a client library for calling Service Brokers that implement the API.

## Links

- [API documentation](https://openservicebroker.typedrest.net/)
