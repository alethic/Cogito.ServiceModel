# Cogito.ServiceModel.DependencyInjection

Lets WCF services be resolved from `Microsoft.Extensions.DependencyInjection`.

## Why

WCF creates service instances itself, so a service class cannot take constructor dependencies. The
usual workaround is a service locator inside the operation, which hides what the service needs and
makes it hard to test.

## Install

```shell
dotnet add package Cogito.ServiceModel.DependencyInjection
```

## Use

With the instance provider in place, a service takes its dependencies normally:

```csharp
public class OrderService : IOrderService
{
    public OrderService(IOrderRepository orders) { ... }
}
```

Each instance is resolved from a scope tied to the WCF instance context, so scoped services behave as
they would per request elsewhere.

## License

MIT.
