# Cogito.ServiceModel

Async open and close for WCF communication objects.

## Why

`ICommunicationObject` exposes `Open`/`Close` and the `Begin`/`End` pair, but nothing task-based — so
opening a channel from async code either blocks a thread or means wrapping the APM methods by hand
every time.

## Install

```shell
dotnet add package Cogito.ServiceModel
```

## Use

```csharp
await channel.OpenAsync();
try
{
    ...
}
finally
{
    await channel.CloseAsync();
}
```

Works on anything implementing `ICommunicationObject` — channels, factories, service hosts.
`ServiceHostExtensions` adds the host-side equivalents.

## License

MIT.
