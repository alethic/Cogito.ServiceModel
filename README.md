# Cogito.ServiceModel

[![Build](https://github.com/alethic/Cogito.ServiceModel/actions/workflows/Cogito.ServiceModel.yml/badge.svg)](https://github.com/alethic/Cogito.ServiceModel/actions/workflows/Cogito.ServiceModel.yml)

Async open and close for WCF communication objects, and WCF services resolved from dependency injection.

## Packages

**[Cogito.ServiceModel](https://www.nuget.org/packages/Cogito.ServiceModel)** — Async open and close for WCF communication objects.

**[Cogito.ServiceModel.DependencyInjection](https://www.nuget.org/packages/Cogito.ServiceModel.DependencyInjection)** — Lets WCF services be resolved from `Microsoft.Extensions.DependencyInjection`.

Each package carries its own README with the detail; the links above go to nuget.org.

## Building

```shell
dotnet restore Cogito.ServiceModel.slnx
dotnet msbuild -p:Configuration=Release Cogito.ServiceModel.dist.msbuildproj
```

Packages are staged into `dist/nuget` and test suites into `dist/tests`; run a suite with
`dotnet test -f <tfm> <path to its assembly>`.

## License

MIT — see [LICENSE](LICENSE).
