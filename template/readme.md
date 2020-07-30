# Open Service Broker Project Template

This template helps you implement a Service Broker according to the [Open Service Broker API](https://www.openservicebrokerapi.org/).  
It uses the [OpenServiceBroker.Server library](https://github.com/TypedRest/OpenServiceBroker#server-library).

## Using the template

Make sure you have the [.NET Core SDK](https://www.microsoft.com/net/download) installed your machine. Then install the template by running the following:

    dotnet new --install OpenServiceBroker.Template::*

To use the template to create a new project:

    dotnet new osb --name MyServiceBroker
    cd MyServiceBroker

## Next steps

The [startup logic](Startup.cs) in the template registers the following services for dependency injection:

- [`CatalogService`](CatalogService.cs) as an implementation of `ICatalogService`.
- [`ServiceInstanceService`](ServiceInstanceService.cs) as an implementation of `IServiceInstanceBlocking`.

The catalog of available services is read from the file [catalog.json](catalog.json).

Provisioned services are stored in a database using Entity Framework and SQLite. The data structures are described by [`DbContext`](DbContext.cs) and [`ServiceInstanceEntity`](ServiceInstanceEntity.cs).

You may wish to:

- Add your own services to [`catalog.json`](catalog.json) or modify [`CatalogService`](CatalogService.cs) fetch service-information from elsewhere.
- Implement the actual provisioning of services in [`ServiceInstanceService`](ServiceInstanceService.cs), potentially removing the database storage.
- Add implementations for `IServiceInstanceDeferred`, `IServiceBindingBlocking` and/or `IServiceBindingDeferred`

See the **[API documentation](https://openservicebroker.typedrest.net/)** for more details.
