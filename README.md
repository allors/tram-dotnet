![Allors TRAM](https://raw.githubusercontent.com/allors/tram-dotnet/main/assets/tram-logo.png)

[![CI](https://github.com/allors/tram-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/allors/tram-dotnet/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Allors.Tram)](https://www.nuget.org/packages/Allors.Tram)
[![License: LGPL-3.0](https://img.shields.io/badge/License-LGPL--3.0-blue.svg)](https://github.com/allors/tram-dotnet/blob/main/LICENSE)

**Transactional memory** for .NET with atomic derivations.

> **Status**: `0.1.x` — early release. The public API may change between minor versions. Semantic Versioning guarantees apply from `1.0` onward.

## Features

- **Handle-based identity** - lightweight `uint` references instead of CLR object references
- **Triple-buffered state** - Primary (working) / Secondary (checkpoint) / Tertiary (committed)
- **Atomic derivation** - `Derive()` runs forward-chaining derivations and persists on success; on failure the transaction is fully rolled back to the last committed state
- **Bidirectional relationships** - OneToOne, OneToMany, ManyToOne, ManyToMany with automatic inverse tracking
- **Change tracking** - fine-grained change sets for created/deleted objects and changed roles
- **Forward-chaining derivations** - reactive computations driven by change sets
- **Configuration-driven schema** - define your domain with a fluent builder

## Install

```shell
dotnet add package Allors.Tram
```

## Quick Example

```csharp
using Allors.Tram;
using Allors.Tram.Schema;
using Allors.Tram.Schema.Config;

var config = new TramSchemaConfigBuilder()
    .AddValueType("String")
    .AddClass("Person")
    .AddAttribute("Person", "String", "FirstName")
    .Build();

var schema = new TramSchema(config);
var tram = new Tram(schema);

var person = (TramClass)schema.TypeByName["Person"];
var firstName = (TramAttribute)person.PropertyBySingularOrPluralName["FirstName"];

var jane = tram.Create(person);
tram.Set(jane, firstName, "Jane");

tram.Derive();
```

See [Getting Started](https://github.com/allors/tram-dotnet/blob/main/docs/getting-started.md) for the full walkthrough — relationships, change tracking, transactions, and guards.

## Documentation

- [Getting Started](https://github.com/allors/tram-dotnet/blob/main/docs/getting-started.md) — tutorial walkthrough
- [Architecture](https://github.com/allors/tram-dotnet/blob/main/docs/architecture.md) — design, layers, and feature reference
- [Changelog](https://github.com/allors/tram-dotnet/blob/main/CHANGELOG.md)

## Contributing

Contributions are welcome. Please read [CONTRIBUTING.md](https://github.com/allors/tram-dotnet/blob/main/CONTRIBUTING.md) and the [Code of Conduct](https://github.com/allors/tram-dotnet/blob/main/CODE_OF_CONDUCT.md).

## License

[LGPL-3.0-only](https://github.com/allors/tram-dotnet/blob/main/LICENSE) © Allors bv
