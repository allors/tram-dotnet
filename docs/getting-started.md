# Getting Started

## Define a Schema

TRAM uses a configuration-driven schema. Define your domain with `TramSchemaConfigBuilder`:

```csharp
using Allors.Tram.Schema;
using Allors.Tram.Schema.Config;

var config = new TramSchemaConfigBuilder()
    .AddValueType("String")
    .AddValueType("Integer")
    .AddValueType("DateTime")
    .AddClass("Organization")
    .AddAttribute("Organization", "String", "Name")
    .AddOneToOne("Organization", "Person", "Owner")
    .AddOneToMany("Organization", "Person", "Employee")
    .AddManyToOne("Organization", "Person", "Manager")
    .AddManyToMany("Organization", "Person", "Customer")
    .AddClass("Person")
    .AddAttribute("Person", "String", "FirstName")
    .AddAttribute("Person", "String", "LastName")
    .AddAttribute("Person", "String", "FullName")
    .Build();

var schema = new TramSchema(config);
```

Interfaces with supertypes are also supported:

```csharp
.AddInterface("I1", ["I12"])
.AddInterface("I12")
.AddClass("C1", ["I1"])
```

## Create Objects and Set Properties

Objects are identified by `Handle` (a lightweight `uint` reference), not CLR objects. Use the `Tram` engine to create and manipulate them:

```csharp
using Allors.Tram;

var tram = new Tram(schema);

var organization = (TramClass)schema.TypeByName["Organization"];
var person = (TramClass)schema.TypeByName["Person"];
var name = (TramAttribute)organization.PropertyBySingularOrPluralName["Name"];
var firstName = (TramAttribute)person.PropertyBySingularOrPluralName["FirstName"];

var acme = tram.Create(organization);
tram.Set(acme, name, "Acme");

var jane = tram.Create(person);
tram.Set(jane, firstName, "Jane");
```

## Relationships

All relationships are bidirectional and have two ends: a writable **role** (`ITramRole`) and a read-only **inverse** (`ITramInverse`). Mutating the role automatically updates the inverse.

### One-to-One

```csharp
var owner = (TramOneToOneRole)organization.PropertyBySingularOrPluralName["Owner"];
var organizationWhereOwner = owner.Inverse;

tram.Set(acme, owner, jane);

// Forward: single Handle
Assert.Equal(jane, tram.Get(acme, owner));
// Inverse: single Handle
Assert.Equal(acme, tram.Get(jane, organizationWhereOwner));
```

### One-to-Many

```csharp
var employees = (TramOneToManyRole)organization.PropertyBySingularOrPluralName["Employees"];
var organizationWhereEmployee = employees.Inverse;

var john = tram.Create(person);

// Add individually
tram.Add(acme, employees, jane);
tram.Add(acme, employees, john);

// Or set the entire collection
tram.Set(acme, employees, [jane, john]);

// Remove
tram.Remove(acme, employees, john);

// Forward: collection
var staff = tram.Get(acme, employees);
// Inverse: single Handle
Assert.Equal(acme, tram.Get(jane, organizationWhereEmployee));
```

### Many-to-One

```csharp
var manager = (TramManyToOneRole)organization.PropertyBySingularOrPluralName["Manager"];
var organizationsWhereManager = manager.Inverse;

tram.Set(acme, manager, jane);

// Forward: single Handle
Assert.Equal(jane, tram.Get(acme, manager));
// Inverse: collection (a single Person can manage many Organizations)
var managed = tram.Get(jane, organizationsWhereManager);
Assert.Contains(acme, managed);
```

### Many-to-Many

```csharp
var customers = (TramManyToManyRole)organization.PropertyBySingularOrPluralName["Customers"];
var organizationsWhereCustomer = customers.Inverse;

tram.Add(acme, customers, jane);

// Both ends are collections
var orgs = tram.Get(jane, organizationsWhereCustomer);
Assert.Contains(acme, orgs);
```

## Transaction Lifecycle

TRAM buffers your mutations and atomically commits them when you call `tram.Derive()`. If a derivation throws, the transaction is rolled back to the last committed state and the exception is rethrown.

```csharp
tram.Set(jane, firstName, "Jane");
tram.Derive();
```

```csharp
try
{
    tram.Set(jane, firstName, "Janet");
    tram.Derive();
}
catch (MaxCyclesExceededException)
{
    // State has been restored to the last committed value automatically.
    Assert.Equal("Jane", tram.Get(jane, firstName));
}
```

## Derivations

Implement `IDerivation` to react to changes during `Derive()`. The engine calls each registered derivation with an `IChangeSet` describing what changed since the last cycle, repeating until no further changes occur.

```csharp
using System.Linq;
using Allors.Tram;
using Allors.Tram.Schema;

public class FullNameDerivation(TramAttribute firstName, TramAttribute lastName, TramAttribute fullName) : IDerivation
{
    public void Derive(ITram tram, IChangeSet changeSet)
    {
        var changed = changeSet.ChangedObjects(firstName).Union(changeSet.ChangedObjects(lastName));

        foreach (var person in changed)
        {
            tram.Set(person, fullName, $"{tram.Get(person, firstName)} {tram.Get(person, lastName)}");
        }
    }
}
```

Derivations are configured at construction time. Build a `Tram` with the derivation registered, mutate, and call `Derive()`:

```csharp
var fullName = (TramAttribute)person.PropertyBySingularOrPluralName["FullName"];

var tram = new Tram(schema, [new FullNameDerivation(firstName, lastName, fullName)]);

var jane = tram.Create(person);
tram.Set(jane, firstName, "Jane");
tram.Set(jane, lastName, "Doe");

tram.Derive();

Assert.Equal("Jane Doe", tram.Get(jane, fullName));
```

See [Architecture](architecture.md) and `DerivationTests.cs` for the full `IChangeSet` query surface and more derivation patterns.
