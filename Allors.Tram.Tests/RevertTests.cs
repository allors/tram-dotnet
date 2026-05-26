// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using Xunit;

public class RevertTests : TestBase
{
    [Fact]
    public void Attribute_RevertedBeforeCheckpoint_HasNoChanges()
    {
        var tram = this.NewTram();
        var m = this.m;

        var john = tram.Create(m.Person);
        tram.Set(john, m.PersonFirstName, "John");
        tram.Checkpoint();

        tram.Set(john, m.PersonFirstName, "Johnny");
        tram.Set(john, m.PersonFirstName, "John");

        var changeSet = tram.Checkpoint();

        Assert.Empty(changeSet.ChangedObjects(m.PersonFirstName));
        Assert.False(changeSet.HasChanges);
    }

    [Fact]
    public void ToOne_RevertedBeforeCheckpoint_HasNoChanges()
    {
        var tram = this.NewTram();
        var m = this.m;

        var (organizationWhereOwner, owner) = m.OrganizationOwner;

        var acme = tram.Create(m.Organization);
        var john = tram.Create(m.Person);
        var jane = tram.Create(m.Person);

        tram.Set(acme, owner, john);
        tram.Checkpoint();

        tram.Set(acme, owner, jane);
        tram.Set(acme, owner, john);

        var changeSet = tram.Checkpoint();

        Assert.Empty(changeSet.ChangedObjects(owner));
        Assert.Empty(changeSet.ChangedObjects(organizationWhereOwner));
        Assert.False(changeSet.HasChanges);
    }

    [Fact]
    public void ToMany_RemoveAndReadd_HasNoChanges()
    {
        var tram = this.NewTram();
        var m = this.m;

        var (c1sWhereC1C1ManyToMany, c1c1ManyToManies) = m.C1C1C1ManyToManies;

        var from = tram.Create(m.C1);
        var b = tram.Create(m.C1);
        var c = tram.Create(m.C1);

        tram.Set(from, c1c1ManyToManies, [b, c]);
        tram.Checkpoint();

        tram.Remove(from, c1c1ManyToManies, c);
        tram.Add(from, c1c1ManyToManies, c);

        var changeSet = tram.Checkpoint();

        Assert.Empty(changeSet.ChangedObjects(c1c1ManyToManies));
        Assert.Empty(changeSet.ChangedObjects(c1sWhereC1C1ManyToMany));
        Assert.False(changeSet.HasChanges);
    }
}
