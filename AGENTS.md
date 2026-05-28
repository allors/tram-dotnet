# AGENTS.md

## Development Notes (important, read before editing)
- Objects are managed by TRAM, always use the API to create or delete objects.
- Relationships are managed by TRAM, always use the API get or set relationships.
- Relationships are bidirectional by design.
- A relationship has two ends, both `ITramRelationEnd`s: a `ITramRole` (forward, writable) and a `ITramInverse` (readonly).
- Attributes are value-type properties (no inverse).
- `Objects()` (on `IReadOnly`) and `Get(handle, ITramToManyRelationEnd)` (roles and inverses) return handles in **no defined order**. To-many values are sets, not lists. The order may differ between implementations, between calls on the same implementation, and between successive runs — it is *not* part of the API contract. Do not rely on it for anything, including nearest-first or insertion-order semantics.
- If you need ordering, carry it as a sortable attribute on an intermediate object, or walk a one-link relation (`Get(handle, ITramToOneRelationEnd)`) at compute time. Never assume the storage layer preserves the order you wrote.
- When a handle is unknown or deleted, for `ValueType` or to-one during `Set`, treat it as `Set(null)` which is `Remove()`.
- When a handle is unknown or deleted, for `ObjectType` during `Set`, `Add`, or `Remove`, ignore the handle.
- Always `checkpoint()` before running derivations; honor change tracking semantics.
- Naming: `TramType` (TramObjectType/TramValueType), `TramObjectType` (TramClass/TramInterface).
- Follow existing patterns; prefer minimal changes in public APIs.
- Follow existing naming and structure; avoid new conventions without reason.
- TRAM is in full initial development, breaking changes are allowed

## Git
- No AI attribution in commits (no "Generated with", "Co-authored-by", or similar trailers)
- Keep commits focused and well-described
- Use conventional commit format: type(scope): description

## Code Style
- Error messages should be actionable

## Workflow
- Follow Test Driven Development
- Never change an existing test unless explicitly instructed to
- Verify builds succeed and tests are green before considering a task complete
- When fixing bugs, always write a failing test first or at least amend an existing test
- When creating a new test, find a suitable existing class to add the test to; avoid creating new test classes
- Keep the mermaid metamodel diagram in `docs/architecture.md` in sync with the schema types under `Allors.Tram/Schema/`.

## Build Commands

Build uses Taskfile (https://taskfile.dev). Run from repository root:

```bash
task compile     # Build all projects
task test        # Run all tests (includes compile)
task pack        # Create NuGet packages (includes test)
task clean       # Clean artifacts and build outputs
```

Override configuration (default is Debug):
```bash
task pack CONFIGURATION=Release
```

Run a single test project:
```bash
dotnet test Allors.Tram.Tests
```

Run specific test:
```bash
dotnet test Allors.Tram.Tests --filter "FullyQualifiedName~OneToOneTests"
```
