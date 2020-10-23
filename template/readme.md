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

[`CatalogService`](CatalogService.cs) reads the list of available services from a static file named [`catalog.json`](catalog.json).  
You can add your services to this file or modify the code to fetch service information from elsewhere.

[`ServiceInstanceService`](ServiceInstanceService.cs) stores provisioned services using Entity Framework and SQLite. The relevant data structures are described by [`DbContext`](DbContext.cs) and [`ServiceInstanceEntity`](ServiceInstanceEntity.cs).  
You will need to add code that does the actual provisioning, optionally replacing the pre-implemented database storage in the process. You can change the service to implement `IServiceInstanceDeferred` instead of `IServiceBindingBlocking` if your provisioning logic takes more than a few seconds to complete.

You may also wish to add and register an implementation for `IServiceBindingBlocking` or `IServiceBindingDeferred`

See the **[API documentation](https://openservicebroker.typedrest.net/)** for more details.
