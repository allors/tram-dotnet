// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using System;
using System.Linq;
using Xunit;

public class ChangeSetTests : TestBase
{
    [Fact]
    public void Attribute_ChangedObjectsContainsTheMutatedObject()
    {
        var tram = this.NewTram();
        var m = this.m;

        var person = m.Person;
        var firstName = m.PersonFirstName;
        var lastName = m.PersonLastName;

        var john = tram.Create(person);
        var jane = tram.Create(person);

        tram.Set(john, firstName, "John");
        tram.Set(john, lastName, "Doe");

        var changeSet = tram.Checkpoint();

        var changedFirstNames = changeSet.ChangedObjects(firstName);
        var changedLastNames = changeSet.ChangedObjects(lastName);

        Assert.Single(changedFirstNames);
        Assert.Single(changedLastNames);
        Assert.Contains(john, changedFirstNames);
        Assert.Contains(john, changedLastNames);

        tram.Set(jane, firstName, "Jane");
        tram.Set(jane, lastName, "Doe");

        var changeSet2 = tram.Checkpoint();

        changedFirstNames = changeSet2.ChangedObjects(firstName);
        changedLastNames = changeSet2.ChangedObjects(lastName);

        Assert.Single(changedFirstNames);
        Assert.Single(changedLastNames);
        Assert.Contains(jane, changedFirstNames);
        Assert.Contains(jane, changedLastNames);
    }

    [Fact]
    public void CreatedObjects_AndIdempotentSetOnManyToMany()
    {
        var tram = this.NewTram();
        var m = this.m;

        var person = m.Person;
        var (_, customers) = m.OrganizationCustomers;

        var john = tram.Create(person);
        var jane = tram.Create(person);

        tram.Set(john, m.PersonFirstName, "John");
        tram.Set(jane, m.PersonFirstName, "Jane");

        var acme = tram.Create(m.Organization);
        tram.Set(acme, m.OrganizationName, "Acme");
        tram.Set(acme, customers, [john, jane]);

        var changeSet = tram.Checkpoint();

        Assert.Equal(3, changeSet.CreatedObjects.Count());
        Assert.Single(changeSet.ChangedObjects(customers));

        // Reorder is a no-op
        tram.Set(acme, customers, [jane, john]);
        changeSet = tram.Checkpoint();
        Assert.Empty(changeSet.ChangedObjects(customers));

        // Clear and restore is a no-op when the net set is the same
        tram.Set(acme, customers, Array.Empty<Handle>());
        tram.Set(acme, customers, [jane, john]);
        changeSet = tram.Checkpoint();
        Assert.Empty(changeSet.ChangedObjects(customers));
    }

    [Fact]
    public void CreatedDeleted_TracksLifecycleAcrossCheckpoints()
    {
        var tram = this.NewTram();
        var m = this.m;

        var john = tram.Create(m.Person);
        var jane = tram.Create(m.Person);
        var acme = tram.Create(m.Organization);

        tram.Delete(john);

        var changeSet = tram.Checkpoint();

        Assert.Equal(2, changeSet.CreatedObjects.Count());
        Assert.Contains(jane, changeSet.CreatedObjects);
        Assert.Contains(acme, changeSet.CreatedObjects);
        Assert.Empty(changeSet.DeletedObjects);

        tram.Delete(acme);
        changeSet = tram.Checkpoint();

        Assert.Empty(changeSet.CreatedObjects);
        Assert.Single(changeSet.DeletedObjects);
        Assert.Contains(acme, changeSet.DeletedObjects);
    }

    [Fact]
    public void ChangedObjectsForOneToOneUsesInverseHandles()
    {
        var tram = this.NewTram();
        var m = this.m;

        var (organizationWhereOwner, owner) = m.OrganizationOwner;

        var acme = tram.Create(m.Organization);
        var john = tram.Create(m.Person);

        tram.Set(acme, owner, john);

        var changeSet = tram.Checkpoint();
        var changedObjects = changeSet.ChangedObjects(organizationWhereOwner);

        Assert.Single(changedObjects);
        Assert.Contains(john, changedObjects);
        Assert.DoesNotContain(acme, changedObjects);
    }

    [Fact]
    public void ChangedObjectsForOneToManyUsesInverseHandles()
    {
        var tram = this.NewTram();
        var m = this.m;

        var (personWhereEmployees, employees) = m.OrganizationEmployees;

        var acme = tram.Create(m.Organization);
        var john = tram.Create(m.Person);
        var jane = tram.Create(m.Person);

        tram.Set(acme, employees, [john, jane]);

        var changeSet = tram.Checkpoint();
        var changedObjects = changeSet.ChangedObjects(personWhereEmployees);

        Assert.Equal(2, changedObjects.Count());
        Assert.Contains(john, changedObjects);
        Assert.Contains(jane, changedObjects);
        Assert.DoesNotContain(acme, changedObjects);
    }

    [Fact]
    public void NewObjectCreatedAndDeletedInSamePeriod_LeavesNoTrace()
    {
        var tram = this.NewTram();
        var m = this.m;

        tram.Checkpoint();

        var john = tram.Create(m.Person);
        tram.Delete(john);

        var changeSet = tram.Checkpoint();

        Assert.Empty(changeSet.CreatedObjects);
        Assert.Empty(changeSet.DeletedObjects);
        Assert.False(changeSet.HasChanges);
    }

    [Fact]
    public void NewObjectMutatedAndDeletedInSamePeriod_StillRecordsAttributeTransition()
    {
        var tram = this.NewTram();
        var m = this.m;

        tram.Checkpoint();

        var john = tram.Create(m.Person);
        tram.Set(john, m.PersonFirstName, "John");
        tram.Delete(john);

        var changeSet = tram.Checkpoint();

        // Object lifecycle cancels out.
        Assert.Empty(changeSet.CreatedObjects);
        Assert.Empty(changeSet.DeletedObjects);

        // Delete cleanup still records the attribute transition back to null.
        Assert.True(changeSet.HasChanges);
        Assert.Contains(john, changeSet.ChangedObjects(m.PersonFirstName));
    }

    [Fact]
    public void OneToMany_StealItemBetweenOwners_BothOwnersAndItemAreTracked()
    {
        var tram = this.NewTram();
        var m = this.m;

        var (personWhereEmployees, employees) = m.OrganizationEmployees;

        var acme = tram.Create(m.Organization);
        var globex = tram.Create(m.Organization);
        var john = tram.Create(m.Person);

        tram.Add(acme, employees, john);
        tram.Checkpoint();

        tram.Set(globex, employees, [john]);

        var changeSet = tram.Checkpoint();

        var changedEmployees = changeSet.ChangedObjects(employees);
        Assert.Equal(2, changedEmployees.Count());
        Assert.Contains(acme, changedEmployees);
        Assert.Contains(globex, changedEmployees);

        var changedInverse = changeSet.ChangedObjects(personWhereEmployees);
        Assert.Single(changedInverse);
        Assert.Contains(john, changedInverse);
    }
}
