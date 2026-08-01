# Open Service Broker API for .NET - Model

DTO classes and exceptions for the [Open Service Broker API](https://www.openservicebrokerapi.org/) specification. This is usually referenced indirectly via [OpenServiceBroker.Client](https://www.nuget.org/packages/OpenServiceBroker.Client/) or [OpenServiceBroker.Server](https://www.nuget.org/packages/OpenServiceBroker.Server/).

## Usage

The DTOs mirror the JSON structures defined by the specification, e.g. `Catalog`, `Service`, `Plan`, `ServiceInstanceProvisionRequest` and `ServiceBindingRequest`.

Non-successful HTTP status codes are mapped to domain-specific exception types (`BrokerException` and derived, such as `AsyncRequiredException`, `ConcurrencyException` and `NotFoundException`), which both the client and server libraries use to signal specification-defined error conditions.

## Links

- [API documentation](https://openservicebroker.typedrest.net/)
