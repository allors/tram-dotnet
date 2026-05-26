// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using System;
using System.Linq;
using Allors.Tram.Frozen;
using Xunit;

public class FrozenTests : TestBase
{
    [Fact]
    public void Schema_MirrorsSource()
    {
        var source = this.NewTram();
        var frozen = new Tram(source);

        Assert.Same(this.schema, frozen.Schema);
    }

    [Fact]
    public void Objects_GetClass_Exists_MirrorSource()
    {
        var source = this.NewTram();
        var m = this.m;

        var acme = source.Create(m.Organization);
        var jane = source.Create(m.Person);
        var john = source.Create(m.Person);

        var frozen = new Tram(source);

        Assert.Equal(source.Objects().OrderBy(v => v).ToArray(), frozen.Objects().OrderBy(v => v).ToArray());
        Assert.Equal(m.Organization, frozen.GetClass(acme));
        Assert.Equal(m.Person, frozen.GetClass(jane));
        Assert.Equal(m.Person, frozen.GetClass(john));
        Assert.True(frozen.Exists(acme));
        Assert.True(frozen.Exists(jane));
        Assert.True(frozen.Exists(john));
    }

    [Fact]
    public void ObjectsOfType_MirrorsSource()
    {
        var source = this.NewTram();
        var m = this.m;

        var acme = source.Create(m.Organization);
        var john = source.Create(m.Person);
        var jane = source.Create(m.Person);
        source.Delete(jane);

        var frozen = new Tram(source);

        Assert.Equal(
            source.ObjectsOfType(m.Organization).OrderBy(v => v).ToArray(),
            frozen.ObjectsOfType(m.Organization).OrderBy(v => v).ToArray());
        Assert.Equal(
            source.ObjectsOfType(m.Person).OrderBy(v => v).ToArray(),
            frozen.ObjectsOfType(m.Person).OrderBy(v => v).ToArray());
        Assert.Equal([acme], frozen.ObjectsOfType(m.Organization).ToArray());
        Assert.Equal([john], frozen.ObjectsOfType(m.Person).ToArray());
    }

    [Fact]
    public void Attribute_MirrorsSource_AndTryGetSemantics()
    {
        var source = this.NewTram();
        var m = this.m;

        var jane = source.Create(m.Person);
        source.Set(jane, m.PersonFirstName, "Jane");

        var john = source.Create(m.Person);
        // john has no firstName set

        var frozen = new Tram(source);

        Assert.Equal("Jane", frozen.Get(jane, m.PersonFirstName));
        Assert.Null(frozen.Get(john, m.PersonFirstName));

        Assert.True(((IReadOnly)frozen).TryGet(jane, m.PersonFirstName, out var janeValue));
        Assert.Equal("Jane", janeValue);

        Assert.False(((IReadOnly)frozen).TryGet(john, m.PersonFirstName, out var johnValue));
        Assert.Null(johnValue);
    }

    [Fact]
    public void ToOne_Role_And_Inverse_MirrorSource()
    {
        var source = this.NewTram();
        var m = this.m;

        var (organizationWhereOwner, owner) = m.OrganizationOwner;

        var acme = source.Create(m.Organization);
        var jane = source.Create(m.Person);
        source.Set(acme, owner, jane);

        var frozen = new Tram(source);

        Assert.Equal(jane, frozen.Get(acme, owner));
        Assert.Equal(acme, frozen.Get(jane, organizationWhereOwner));

        Assert.True(((IReadOnly)frozen).TryGet(acme, owner, out var roleItem));
        Assert.Equal(jane, roleItem);

        Assert.True(((IReadOnly)frozen).TryGet(jane, organizationWhereOwner, out var inverseItem));
        Assert.Equal(acme, inverseItem);

        // unset to-one returns null Handle, TryGet returns false
        var orphan = source.Create(m.Organization);
        var frozenWithOrphan = new Tram(source);
        Assert.True(frozenWithOrphan.Get(orphan, owner).IsNull);
        Assert.False(((IReadOnly)frozenWithOrphan).TryGet(orphan, owner, out var orphanRole));
        Assert.True(orphanRole.IsNull);
    }

    [Fact]
    public void ToMany_Role_And_Inverse_MirrorSource()
    {
        var source = this.NewTram();
        var m = this.m;

        var (personWhereEmployees, employees) = m.OrganizationEmployees;

        var acme = source.Create(m.Organization);
        var jane = source.Create(m.Person);
        var john = source.Create(m.Person);
        source.Add(acme, employees, jane);
        source.Add(acme, employees, john);

        var frozen = new Tram(source);

        var employeesFromFrozen = frozen.Get(acme, employees).ToArray();
        Assert.Equal(2, employeesFromFrozen.Length);
        Assert.Contains(jane, employeesFromFrozen);
        Assert.Contains(john, employeesFromFrozen);

        Assert.Equal(acme, frozen.Get(jane, personWhereEmployees));
        Assert.Equal(acme, frozen.Get(john, personWhereEmployees));

        Assert.True(((IReadOnly)frozen).TryGet(acme, employees, out var roleItems));
        Assert.Equal(2, roleItems.Count());

        // empty to-many returns empty enumerable, TryGet returns false
        var bob = source.Create(m.Person);
        var frozenWithBob = new Tram(source);
        Assert.Empty(frozenWithBob.Get(bob, employees));
        Assert.False(((IReadOnly)frozenWithBob).TryGet(bob, employees, out var bobItems));
        Assert.Empty(bobItems);
    }

    [Fact]
    public void Frozen_DoesNotObserve_SourceMutationsAfterFreezing()
    {
        var source = this.NewTram();
        var m = this.m;

        var (_, owner) = m.OrganizationOwner;
        var (_, employees) = m.OrganizationEmployees;

        var acme = source.Create(m.Organization);
        source.Set(acme, m.OrganizationName, "Acme");
        var jane = source.Create(m.Person);
        source.Set(acme, owner, jane);
        source.Add(acme, employees, jane);

        var frozen = new Tram(source);

        // Mutate source after freezing
        source.Set(acme, m.OrganizationName, "AcmeChanged");
        source.Set(acme, owner, default);
        source.Remove(acme, employees, jane);
        var newcomer = source.Create(m.Person);
        source.Delete(jane);

        // Frozen still reflects pre-freeze state
        Assert.Equal("Acme", frozen.Get(acme, m.OrganizationName));
        Assert.Equal(jane, frozen.Get(acme, owner));
        Assert.Equal([jane], frozen.Get(acme, employees).ToArray());
        Assert.True(frozen.Exists(jane));
        Assert.False(frozen.Exists(newcomer));
    }

    [Fact]
    public void Assert_NullHandle_Throws()
    {
        var source = this.NewTram();
        var frozen = new Tram(source);

        Assert.Throws<ArgumentException>(() => frozen.GetClass(default));
        Assert.Throws<ArgumentException>(() => frozen.Get(default, this.m.PersonFirstName));
    }

    [Fact]
    public void Assert_UnknownHandle_Throws()
    {
        var source = this.NewTram();
        var frozen = new Tram(source);

        Handle unknown = 999;
        Assert.Throws<ArgumentException>(() => frozen.GetClass(unknown));
        Assert.Throws<ArgumentException>(() => frozen.Get(unknown, this.m.PersonFirstName));
    }

    [Fact]
    public void DoesNotImplementIReadWrite()
    {
        var source = this.NewTram();
        var frozen = new Tram(source);

        Assert.IsAssignableFrom<IReadOnly>(frozen);
        Assert.False(typeof(IReadWrite).IsAssignableFrom(typeof(Tram)));
    }

    [Fact]
    public void EmptySource_ProducesEmptyFrozen()
    {
        var source = this.NewTram();
        var frozen = new Tram(source);

        Assert.Empty(frozen.Objects());
        Assert.False(frozen.Exists(1));
    }
}
