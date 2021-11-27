# Open Service Broker API for .NET

[![Build](https://github.com/TypedRest/OpenServiceBroker/workflows/Build/badge.svg?branch=master)](https://github.com/TypedRest/OpenServiceBroker/actions?query=workflow%3ABuild)
[![API documentation](https://img.shields.io/badge/api-docs-orange.svg)](https://openservicebroker.typedrest.net/)

This project provides both a server and a client .NET library for the [Open Service Broker API](https://www.openservicebrokerapi.org/) specification. This specification allows developers, ISVs, and SaaS vendors a single, simple, and elegant way to deliver services to applications running within cloud native platforms such as Cloud Foundry, OpenShift, and Kubernetes.

The **[Server Library](#server-library)** implements the API for you using ASP.NET Core. You simply need to provide implementations for a few interfaces, shielded from the HTTP-related details.

The **[Client Library](#client-library)** allows you to call Service Brokers that implement the API using idiomatic C# interfaces and type-safe DTOs.

## Server Library

[![OpenServiceBroker.Server](https://img.shields.io/nuget/v/OpenServiceBroker.Server.svg)](https://www.nuget.org/packages/OpenServiceBroker.Server/)

Set up a regular ASP.NET Core 3.1, 5.0 or 6.0 project and add the NuGet package [`OpenServiceBroker.Server`](https://www.nuget.org/packages/OpenServiceBroker.Server/). Then implement the following interfaces:
- [`ICatalogService`](src/Server/Catalogs/ICatalogService.cs) (optionally also [`IETagProvider`](src/Server/Catalogs/IETagProvider.cs) and/or [`ILastModifiedProvider`](src/Server/Catalogs/ILastModifiedProvider.cs) on the same class)
- either [`IServiceInstanceBlocking`](src/Server/Instances/IServiceInstanceBlocking.cs) or [`IServiceInstanceDeferred`](src/Server/Instances/IServiceInstanceDeferred.cs) or both
- either [`IServiceBindingBlocking`](src/Server/Bindings/IServiceBindingBlocking.cs) or [`IServiceBindingDeferred`](src/Server/Bindings/IServiceBindingDeferred.cs) or both

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

You can use the **[project template](template/)** to quickly set up a pre-configured ASP.NET Core 5.0 project with `OpenServiceBroker.Server`.

### Versioning

The Server Library inspects the `X-Broker-API-Version` header for all requests (as defined in the specification). Currently it accepts all versions from `2.0` to `2.16`.

## Client Library

[![OpenServiceBroker.Client](https://img.shields.io/nuget/v/OpenServiceBroker.Client.svg)](https://www.nuget.org/packages/OpenServiceBroker.Client/)

Add the NuGet package [`OpenServiceBroker.Client`](https://www.nuget.org/packages/OpenServiceBroker.Client/) to your project. You can then create an instance of the client like this:

```csharp
var client = new OpenServiceBrokerClient(new Uri("http://example.com/"));
```

### Asynchronous Operations

All operations that result in HTTP request are `async`. Non-successful HTTP status codes are mapped to domain-specific exception types ([`BrokerException`](src/Model/Errors/BrokerException.cs) and derived). Refer to the libraries XML documentation for details on which exceptions to expect on which invocations.

The Open Service Broker API specification allows for both synchronous/blocking and asynchronous/incomplete/deferred operations. To avoid confusion with the C# language concept of `async` this library uses the terms "blocking" and "deferred" to describe these API features.

Instances of [`OpenServiceBrokerClient`](src/Client/OpenServiceBrokerClient.cs) have three properties that expose the same functionality in different ways:

- `.ServiceInstancesBlocking` allows you to request blocking responses from the server. However, you may encounter [`AsyncRequiredException`](src/Model/Errors/AsyncRequiredException.cs) if the server does not support blocking operations.
- `.ServiceInstancesDeferred` allows you to request deferred responses from the server. However, you have to manually handle waiting/polling for the completion of operations.
- `.ServiceInstancesPolling` combines the advantages of both. It requests deferred responses from the server and transparently handles the waiting/polling for you. It is the recommended option for most use-cases.

### Samples

Read the catalog:

```csharp
var result = await client.Catalog.ReadAsync();
```

Provision a service instance:

```csharp
var result = await client.ServiceInstancesPolling["123"].ProvisionAsync(new ServiceInstanceProvisionRequest
{
    ServiceId = "abc",
    PlanId = "xyz",
    Context = new JObject
    {
        {"platform", "myplatform"}
    },
    Parameters = new JObject
    {
        {"some_option", "some value"}
    }
});
```

Fetch a service instance:

```csharp
var result = await client.ServiceInstancesPolling["123"].FetchAsync();
```

Update a service instance:

```csharp
var result = await client.ServiceInstancesPolling["123"].UpdateAsync(new ServiceInstanceUpdateRequest
{
    ServiceId = "abc",
    PlanId = "xyz",
    Context = new JObject
    {
        {"platform", "myplatform"}
    },
    Parameters = new JObject
    {
        {"some_option", "some value"}
    }
});
```

Deprovision a service instance:

```csharp
await client.ServiceInstancesPolling["123"].DeprovisionAsync(serviceId: "abc", planId: "xyz");
```

Create a service binding:

```csharp
var result = await client.ServiceInstancesPolling["123"].ServiceBindings["456"].ProvisionAsync(new ServiceBindingRequest
{
    ServiceId = "abc",
    PlanId = "xyz",
    BindResource = new ServiceBindingResourceObject
    {
        AppGuid = "e490c9df-6627-4699-8db8-55edc2a88e58"
    },
    Context = new JObject
    {
        {"platform", "myplatform"}
    },
    Parameters = new JObject
    {
        {"some_option", "some value"}
    }
});
```

Fetch a service binding:

```csharp
var result = await client.ServiceInstancesPolling["123"].ServiceBindings["456"].FetchAsync();
```

Delete a service binding:

```csharp
await client.ServiceInstancesPolling["123"].ServiceBindings["456"].UnbindAsync(serviceId: "abc", planId: "xyz");
```

### Versioning

The Client Library specifies the API version it expects by setting the `X-Broker-API-Version` header for all requests (as defined in the specification).

Currently the Client Library supports the `2.16` feature set but defaults to setting the version header to `2.13` for greater compatibility with older brokers. If the broker you are calling expects a different version and you are sure your request is compliant with that version of the specification you can override this:

```csharp
client.SetApiVersion(new ApiVersion(2, 16));
```

## Building

The source code is in [`src/`](src/), config for building the API documentation is in [`doc/`](doc/) and generated build artifacts are placed in `artifacts/`. The source code does not contain version numbers. Instead the version is determined during CI using [GitVersion](https://gitversion.net/).

To build run `.\build.ps1` or `./build.sh` (.NET SDK is automatically downloaded if missing using [0install](https://0install.net/)).
 
## Contributing

We welcome contributions to this project such as bug reports, recommendations and pull requests.

This repository contains an [EditorConfig](http://editorconfig.org/) file. Please make sure to use an editor that supports it to ensure consistent code style, file encoding, etc.. For full tooling support for all style and naming conventions consider using JetBrains' [ReSharper](https://www.jetbrains.com/resharper/) or [Rider](https://www.jetbrains.com/rider/) products.
