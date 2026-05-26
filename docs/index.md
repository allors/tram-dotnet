# Allors TRAM

**Transactional memory framework** for .NET with handle-based object identity and triple-buffered state management.

## Quick links

- [Getting Started](getting-started.md) — build a small TRAM application end-to-end.
- [Architecture](architecture.md) — design, layers, and core concepts.
- [API Reference](api/) — auto-generated from XML doc comments.
- [GitHub repository](https://github.com/allors/tram-dotnet)
- [NuGet: Allors.Tram](https://www.nuget.org/packages/Allors.Tram)

## Install

```bash
dotnet add package Allors.Tram
```

## Concepts at a glance

- **Handles** — lightweight `uint` references to objects, stable for their lifetime.
- **Triple-buffered state** — Primary (working) / Secondary (checkpoint) / Tertiary (committed).
- **Transactions** — atomic `Derive()` runs forward-chaining derivations and persists on success; on failure the transaction is rolled back to the last committed state.
- **Schema** — fluent configuration of value types, classes, interfaces, attributes, and four relation multiplicities.
- **Change tracking** — fine-grained `IChangeSet` for created/deleted objects and changed roles, ready for forward-chaining derivations.

## Status

`0.1.x` — early release. The public API may change between minor versions. Semantic Versioning guarantees apply from `1.0` onward.

## License

LGPL-3.0-only. See [LICENSE](https://github.com/allors/tram-dotnet/blob/main/LICENSE).
