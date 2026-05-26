# Contributing to Allors TRAM

Thanks for your interest in contributing. This document covers the development workflow and conventions.

## Code of Conduct

This project follows the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold it. Report unacceptable behavior to `legal@allors.com`.

## Contributor License Agreement

Before we can accept your contribution, you must sign an Apache-style Contributor License Agreement (CLA).

Individual and corporate CLA forms are available — email `legal@allors.com` to request the form and for any questions. Pull requests from contributors without a CLA on file will be held until the signed CLA is received.

## Development Setup

Requirements:

- .NET 10 SDK.
- [Task](https://taskfile.dev) (the build runner).

Common commands (run from the repo root):

```bash
task compile     # Build all projects
task test        # Run all tests (depends on compile)
task pack        # Create NuGet packages (depends on test)
task clean       # Remove build artifacts
```

To run a single test project:

```bash
dotnet test Allors.Tram.Tests
```

To run a specific test:

```bash
dotnet test Allors.Tram.Tests --filter "FullyQualifiedName~OneToOneTests"
```

## Workflow — trunk-based development

The `main` branch is the trunk. All work lands on `main` via pull request.

1. Fork the repo and create a topic branch off `main`.
2. Make your changes following the conventions below.
3. Open a PR against `main`. Keep PRs focused — one logical change per PR.
4. CI will build and test.
5. After review and approval, a maintainer merges (squash or rebase, no merge commits).

There are no long-lived feature branches. Releases are cut by tagging a commit on `main` (see [Releasing](#releasing)).

For non-trivial changes, open an issue first to discuss the approach.

## Commit Messages

Use [Conventional Commits](https://www.conventionalcommits.org/):

```
type(scope): short description

Longer body if needed.
```

Common types: `feat`, `fix`, `refactor`, `docs`, `test`, `chore`, `build`, `ci`. Scope is optional; use it to point at a subsystem (`feat(schema): ...`, `fix(tram): ...`).

## Code Conventions

- Follow patterns already in the codebase. Match existing naming and structure.
- TRAM manages objects and relationships — always use the public API to create, delete, or read/write. Never bypass it.
- Relationships are bidirectional: an `ITramRole` has a paired `ITramInverse`. Don't add one without the other.
- Attributes are value-type properties (no inverse).
- Treat all warnings as errors (`TreatWarningsAsErrors=true` is on). Fix Roslynator and analyzer warnings rather than suppressing.
- Public API additions require XML documentation comments.

## Testing

We follow Test-Driven Development:

- For new functionality, write a failing test first.
- For bug fixes, add (or amend) a test that reproduces the bug before fixing.
- Avoid creating new test classes — add tests to a relevant existing class where possible.
- Never modify an existing test unless the test itself is wrong or the API it tested has been intentionally removed.

## Releasing (maintainers only)

The release workflow (`.github/workflows/release.yml`) is tag-driven: pushing a tag matching `v*.*.*` or `v*.*.*-*` to `origin` triggers a build, NuGet.org publish via OIDC trusted publishing, and a GitHub Release.

Each release is a single commit that pins the version in `version.json`, followed by a matching annotated tag.

### Cut a pre-release (alpha / beta / rc)

```bash
git switch main && git pull --ff-only

# Edit version.json: "version": "0.1.0-alpha.1"
git add version.json CHANGELOG.md
git commit -m "chore(release): v0.1.0-alpha.1"

git tag -a v0.1.0-alpha.1 -m "v0.1.0-alpha.1"
git push origin main --follow-tags
```

### Cut a stable release

```bash
git switch main && git pull --ff-only

# Edit version.json: "version": "0.1.0"
# Move CHANGELOG entries from [Unreleased]/pre-release sections into the new version section
git add version.json CHANGELOG.md
git commit -m "chore(release): v0.1.0"

git tag -a v0.1.0 -m "v0.1.0"
git push origin main --follow-tags
```

### After the tag push

The release workflow waits for approval on the `nuget-release` GitHub Environment before publishing. Approve in the GitHub UI; on success the package appears on NuGet.org and a GitHub Release is created with `.nupkg` + `.snupkg` attached.

After a stable release, bump `version.json` to the next dev stem (e.g. `0.1.1-alpha`) on `main` so subsequent CI builds produce distinguishable versions.

## Questions

Open a [Discussion](https://github.com/allors/tram-dotnet/discussions) for design questions or general help. Use [Issues](https://github.com/allors/tram-dotnet/issues) for bugs and feature requests.
