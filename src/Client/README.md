# Open Service Broker API for .NET - Client

Client library for calling the [Open Service Broker API](https://www.openservicebrokerapi.org/) specification using idiomatic C# interfaces and type-safe DTOs.

    dotnet add package OpenServiceBroker.Client

## Usage

```csharp
var client = new OpenServiceBrokerClient(new Uri("http://example.com/"));
```

All operations that result in a HTTP request are `async`. Non-successful HTTP status codes are mapped to domain-specific exception types (`BrokerException` and derived). Refer to the library's XML documentation for details on which exceptions to expect on which invocations.

The Open Service Broker API specification allows for both synchronous/blocking and asynchronous/incomplete/deferred operations. To avoid confusion with the C# language concept of `async` this library uses the terms "blocking" and "deferred" to describe these API features.

Instances of `OpenServiceBrokerClient` have three properties that expose the same functionality in different ways:

- `.ServiceInstancesBlocking` allows you to request blocking responses from the server. However, you may encounter `AsyncRequiredException` if the server does not support blocking operations.
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

The client library specifies the API version it expects by setting the `X-Broker-API-Version` header for all requests (as defined in the specification).

Currently the client library supports the `2.16` feature set but defaults to setting the version header to `2.13` for greater compatibility with older brokers. If the broker you are calling expects a different version and you are sure your request is compliant with that version of the specification you can override this:

```csharp
client.SetApiVersion(new ApiVersion(2, 16));
```

## Related packages

- [OpenServiceBroker.Model](https://www.nuget.org/packages/OpenServiceBroker.Model/) provides the DTOs and exceptions used by this library.
- [OpenServiceBroker.Server](https://www.nuget.org/packages/OpenServiceBroker.Server/) implements the same API using ASP.NET Core.

## Links

- [API documentation](https://openservicebroker.typedrest.net/)
