// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using System;
using System.Linq;
using Xunit;

public class OneToManyTests : TestBase
{
    [Fact]
    public void Initial_RoleIsEmptyAndInverseIsNull()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToManies;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        Assert.Empty(tram.Get(from, role));
        Assert.Equal(default, tram.Get(to, inverse));
    }

    [Fact]
    public void Add_PopulatesInverseAndIsIdempotent()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToManies;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Add(from, role, to);
        tram.Add(from, role, to);

        Assert.Single(tram.Get(from, role));
        Assert.Contains(to, tram.Get(from, role));
        Assert.Equal(from, tram.Get(to, inverse));
    }

    [Fact]
    public void AddMultiple_GrowsCollectionAndInversesPointToOwner()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToManies;

        var from = tram.Create(this.m.C1);
        var a = tram.Create(this.m.C1);
        var b = tram.Create(this.m.C1);
        var c = tram.Create(this.m.C1);

        tram.Add(from, role, a);
        tram.Add(from, role, b);
        tram.Add(from, role, c);

        var collection = tram.Get(from, role);
        Assert.Equal(3, collection.Count());
        Assert.Contains(a, collection);
        Assert.Contains(b, collection);
        Assert.Contains(c, collection);
        Assert.Equal(from, tram.Get(a, inverse));
        Assert.Equal(from, tram.Get(b, inverse));
        Assert.Equal(from, tram.Get(c, inverse));
    }

    [Fact]
    public void Remove_ClearsInverseAndIsIdempotent()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToManies;

        var from = tram.Create(this.m.C1);
        var a = tram.Create(this.m.C1);
        var b = tram.Create(this.m.C1);

        tram.Add(from, role, a);
        tram.Add(from, role, b);

        tram.Remove(from, role, b);
        tram.Remove(from, role, b);

        Assert.Single(tram.Get(from, role));
        Assert.Contains(a, tram.Get(from, role));
        Assert.Equal(default, tram.Get(b, inverse));
    }

    [Fact]
    public void RemoveFromWrongOwner_IsNoOp()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToManies;

        var from = tram.Create(this.m.C1);
        var fromAnother = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Add(from, role, to);
        tram.Remove(fromAnother, role, to);

        Assert.Single(tram.Get(from, role));
        Assert.Contains(to, tram.Get(from, role));
        Assert.Equal(from, tram.Get(to, inverse));
    }

    [Fact]
    public void Set_ReplacesCollectionEntirely()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToManies;

        var from = tram.Create(this.m.C1);
        var a = tram.Create(this.m.C1);
        var b = tram.Create(this.m.C1);

        tram.Set(from, role, [a]);
        tram.Set(from, role, [b]);

        Assert.Single(tram.Get(from, role));
        Assert.Contains(b, tram.Get(from, role));
        Assert.Equal(default, tram.Get(a, inverse));
        Assert.Equal(from, tram.Get(b, inverse));
    }

    [Fact]
    public void SetEmpty_ClearsCollectionAndInverses()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToManies;

        var from = tram.Create(this.m.C1);
        var a = tram.Create(this.m.C1);
        var b = tram.Create(this.m.C1);

        tram.Set(from, role, [a, b]);
        tram.Set(from, role, Array.Empty<Handle>());

        Assert.Empty(tram.Get(from, role));
        Assert.Equal(default, tram.Get(a, inverse));
        Assert.Equal(default, tram.Get(b, inverse));
    }

    [Fact]
    public void SetWithNullsInArray_FiltersThemOut()
    {
        var tram = this.NewTram();
        var (_, role) = this.m.C1C1C1OneToManies;

        var from = tram.Create(this.m.C1);
        var a = tram.Create(this.m.C1);
        var b = tram.Create(this.m.C1);

        var withNulls = new Handle[5];
        withNulls[1] = a;
        withNulls[3] = b;

        tram.Set(from, role, withNulls);

        Assert.Equal(2, tram.Get(from, role).Count());
        Assert.Contains(a, tram.Get(from, role));
        Assert.Contains(b, tram.Get(from, role));
    }

    [Fact]
    public void CheckRoleIsAssignable()
    {
        var tram = this.NewTram();
        var m = this.m;

        var c1A = tram.Create(m.C1);
        var c1B = tram.Create(m.C1);
        var c2A = tram.Create(m.C2);

        Assert.Throws<ArgumentException>(() => tram.Add(c1A, m.C1C1C2OneToManies, c1B));
        Assert.Throws<ArgumentException>(() => tram.Set(c1A, m.C1C1C2OneToManies, [c1B]));
        Assert.Throws<ArgumentException>(() => tram.Add(c1A, m.C1C1I2OneToManies, c1B));
        Assert.Throws<ArgumentException>(() => tram.Remove(c1A, m.C1C1I2OneToManies, c1B));
        Assert.Throws<ArgumentException>(() => tram.Set(c1A, m.C1C1I2OneToManies, [c1B]));
        Assert.Throws<ArgumentException>(() => tram.Add(c1A, m.C3C3C1OneToManies, c2A));
        Assert.Throws<ArgumentException>(() => tram.Set(c1A, m.C3C3C1OneToManies, [c2A]));
    }
}
