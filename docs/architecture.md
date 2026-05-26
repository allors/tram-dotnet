# Architecture

## Layer Structure

```
Allors.Tram   → Core engine + interfaces (Tram, ITram, Handle, IChangeSet) and
                schema (TramSchema, TramType, TramObjectType, TramRole, TramInverse, Mermaid)
```

## Core Concepts

### Handle

Objects are referenced by `Handle`, a lightweight `uint`-based identifier. Handles are created by `tram.Create()` and remain stable for the object's lifetime. This avoids the overhead and GC pressure of CLR object references.

### Triple-Buffered State

State is managed across three layers:

| Layer | Purpose |
|---|---|
| **Primary** | Working state - all reads and writes operate here |
| **Secondary** | Checkpoint state - frozen by `Checkpoint()` |
| **Tertiary** | Committed state - frozen by a successful `Derive()` |

`Checkpoint()` (internal) advances Primary → Secondary and returns a `IChangeSet`.
`Derive()` runs derivations to quiescence and advances Secondary → Tertiary. If a derivation throws, Primary and Secondary are restored from Tertiary and the exception is rethrown — the transaction is atomic.

### Schema

The schema is defined at startup via `TramSchemaConfigBuilder` and is immutable at runtime. It defines:

- **Value types** - `TramValueType` for scalars (String, Integer, DateTime, Boolean, ...)
- **Object types** - `TramClass` and `TramInterface` (both extend `TramObjectType`), with optional inheritance
- **Properties** - `TramAttribute` for value-typed properties, and bidirectional relations split into a writable **role** (`ITramRole`) and a read-only **inverse** (`ITramInverse`). Both ends are `ITramRelationEnd`s.

### Relationships

| Multiplicity | Role (writable) | Inverse (read-only) |
|---|---|---|
| OneToOne | single `Handle` | single `Handle` |
| OneToMany | `IHandles` collection | single `Handle` |
| ManyToOne | single `Handle` | `IHandles` collection |
| ManyToMany | `IHandles` collection | `IHandles` collection |
| Attribute | `object?` (scalar value) | *(no inverse)* |

Roles (`ITramRole`) are mutated with `Set`, `Add`, `Remove`; inverses (`ITramInverse`) are kept in sync automatically and are read-only. From either end, `OtherEnd` navigates to the other end; on the strongly-typed role classes the same navigation is also exposed as `Inverse`.

### Change Sets

`IChangeSet` tracks what changed between checkpoints:

- `CreatedObjects` / `DeletedObjects` - created or deleted handles
- `ChangedObjects(attribute)` - which objects had a given attribute modified
- `ChangedObjects(relationEnd)` - which objects had a given role or inverse modified
- `ChangedAttributes` / `ChangedRoles` / `ChangedInverses` - which property types had any changes

### Derivations

Derivations are forward-chaining computations driven by change sets. Register them with the `Tram` constructor; `Derive()` runs them to quiescence and persists the result atomically.

- `IDerivation` — single-method interface (`Derive(ITram, IChangeSet)`) a derivation implements.
- `Tram(schema, derivations, maxCycles)` — wires derivations into the engine. The orchestrator iterates: checkpoint → invoke each derivation → checkpoint, until the change set is empty (quiescent).
- `MaxCyclesExceededException` — thrown when iteration exceeds `maxCycles`, so cyclic or non-converging derivations fail loudly. On the throw, `Derive()` rolls the transaction back to the last committed state.

`DerivationTests.cs` and `DerivationOverrideTests.cs` exercise the pattern end-to-end (e.g., `FullName` derived from `FirstName` + `LastName`, with chained dependencies and cycle detection).

### Mermaid Class Diagrams

`Allors.Tram.Schema.Diagrams.TramSchemaMermaidExtensions` renders a `TramSchema` as a [Mermaid](https://mermaid.js.org/) class diagram. Inheritance edges render as `<|--`; roles render as `o--` with the role name as the edge label.

```csharp
using Allors.Tram.Schema.Diagrams;

var diagram = schema.ToMermaidClassDiagram(new MermaidClassDiagramOptions
{
    Title = "Domain",
    OneMultiplicity = "1",
    ManyMultiplicity = "*",
});
```

`MermaidClassDiagramOptions` knobs:

| Option | Effect |
|---|---|
| `Title` | YAML frontmatter title above the diagram |
| `OneMultiplicity` | Label rendered on to-one ends (e.g., `"1"`) |
| `ManyMultiplicity` | Label rendered on to-many ends (e.g., `"*"`) |

For a schema with `Organization`, `Person`, and a `OneToMany("Organization", "Person", "Employee")` relation, the default output is:

```
classDiagram
    class Organization
    Organization o-- Person : Employees
    class Person
```

## Packages

| Package | Description |
|---|---|
| `Allors.Tram` | Core engine, interfaces, handles, change tracking, schema types, config builder, Mermaid diagram extension |

## Build

Build uses [Task](https://taskfile.dev). Each main target depends on prior steps (`test` runs `compile`, `pack` runs `test`).

```shell
task compile     # Build all projects
task test        # Run all tests
task pack        # Create NuGet packages
task clean       # Clean artifacts and build outputs
```

Override the configuration (defaults to `Debug`):

```shell
task pack CONFIGURATION=Release
```

Run a single test project:

```shell
dotnet test Allors.Tram.Tests
```

Run a specific test:

```shell
dotnet test Allors.Tram.Tests --filter "FullyQualifiedName~OneToOneTests"
```

## Project Layout

```
Allors.Tram/         # Core engine, interfaces, change tracking; Schema/ holds schema types, config builder, Mermaid extension
Allors.Tram.Tests/   # Engine and schema tests (schema tests under Schema/)
```
