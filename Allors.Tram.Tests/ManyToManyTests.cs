// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using System;
using System.Linq;
using Xunit;

public class ManyToManyTests : TestBase
{
    [Fact]
    public void Initial_BothEndsAreEmpty()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToManies;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        Assert.Empty(tram.Get(from, role));
        Assert.Empty(tram.Get(to, inverse));
    }

    [Fact]
    public void Add_PopulatesBothEndsAndIsIdempotent()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToManies;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Add(from, role, to);
        tram.Add(from, role, to);

        Assert.Single(tram.Get(from, role));
        Assert.Contains(to, tram.Get(from, role));
        Assert.Single(tram.Get(to, inverse));
        Assert.Contains(from, tram.Get(to, inverse));
    }

    [Fact]
    public void Set_AndSetEmpty_ClearsBothEnds()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToManies;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Set(from, role, [to]);
        tram.Set(from, role, Array.Empty<Handle>());

        Assert.Empty(tram.Get(from, role));
        Assert.Empty(tram.Get(to, inverse));
    }

    [Fact]
    public void TwoFromsShareOneTo_InverseHasBoth()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToManies;

        var from = tram.Create(this.m.C1);
        var fromAnother = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Add(from, role, to);
        tram.Add(fromAnother, role, to);

        var inverses = tram.Get(to, inverse).ToArray();
        Assert.Equal(2, inverses.Length);
        Assert.Contains(from, inverses);
        Assert.Contains(fromAnother, inverses);
    }

    [Fact]
    public void OneFromSharedToMultipleTos_RoleHasAll()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToManies;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);
        var toAnother = tram.Create(this.m.C1);

        tram.Set(from, role, [to, toAnother]);

        var roles = tram.Get(from, role).ToArray();
        Assert.Equal(2, roles.Length);
        Assert.Contains(to, roles);
        Assert.Contains(toAnother, roles);
        Assert.Equal([from], tram.Get(to, inverse).ToArray());
        Assert.Equal([from], tram.Get(toAnother, inverse).ToArray());
    }

    [Fact]
    public void Remove_OnlyAffectsOneAssociation()
    {
        var tram = this.NewTram();
        var m = this.m;
        var (inverse, role) = m.C1C1C1ManyToManies;

        var from = tram.Create(m.C1);
        var fromAnother = tram.Create(m.C1);
        var to = tram.Create(m.C1);

        tram.Add(from, role, to);
        tram.Add(fromAnother, role, to);

        tram.Remove(from, role, to);

        Assert.Empty(tram.Get(from, role));
        var inverses = tram.Get(to, inverse).ToArray();
        Assert.Single(inverses);
        Assert.Equal(fromAnother, inverses[0]);
    }

    [Fact]
    public void Remove_LeavesOtherRolesIntact()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToManies;

        var from = tram.Create(this.m.C1);
        var to1 = tram.Create(this.m.C1);
        var to2 = tram.Create(this.m.C1);

        tram.Add(from, role, to1);
        tram.Add(from, role, to2);

        tram.Remove(from, role, to1);

        var roles = tram.Get(from, role).ToArray();
        Assert.Single(roles);
        Assert.Equal(to2, roles[0]);
        Assert.Empty(tram.Get(to1, inverse));
        Assert.Equal([from], tram.Get(to2, inverse).ToArray());
    }

    [Fact]
    public void Remove_InComplexBipartiteGraph_MaintainsConsistency()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToManies;

        var from1 = tram.Create(this.m.C1);
        var from2 = tram.Create(this.m.C1);
        var to1 = tram.Create(this.m.C1);
        var to2 = tram.Create(this.m.C1);

        tram.Add(from1, role, to1);
        tram.Add(from1, role, to2);
        tram.Add(from2, role, to1);

        tram.Remove(from1, role, to1);

        Assert.Equal([to2], tram.Get(from1, role).ToArray());
        Assert.Equal([from2], tram.Get(to1, inverse).ToArray());
        Assert.Equal([from1], tram.Get(to2, inverse).ToArray());
    }

    [Fact]
    public void CheckRoleIsAssignable()
    {
        var tram = this.NewTram();
        var m = this.m;

        var c1A = tram.Create(m.C1);
        var c1B = tram.Create(m.C1);
        var c2A = tram.Create(m.C2);

        Assert.Throws<ArgumentException>(() => tram.Add(c1A, m.C1C1C2ManyToManies, c1B));
        Assert.Throws<ArgumentException>(() => tram.Remove(c1A, m.C1C1C2ManyToManies, c1B));
        Assert.Throws<ArgumentException>(() => tram.Set(c1A, m.C1C1C2ManyToManies, [c1B]));
        Assert.Throws<ArgumentException>(() => tram.Add(c1A, m.C1C1I2ManyToManies, c1B));
        Assert.Throws<ArgumentException>(() => tram.Remove(c1A, m.C1C1I2ManyToManies, c1B));
        Assert.Throws<ArgumentException>(() => tram.Set(c1A, m.C1C1I2ManyToManies, [c1B]));
        Assert.Throws<ArgumentException>(() => tram.Add(c1A, m.C3C3C1ManyToManies, c2A));
        Assert.Throws<ArgumentException>(() => tram.Remove(c1A, m.C3C3C1ManyToManies, c2A));
        Assert.Throws<ArgumentException>(() => tram.Set(c1A, m.C3C3C1ManyToManies, [c2A]));
    }
}
