// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using System;
using System.Linq;
using Xunit;

public class ManyToOneTests : TestBase
{
    [Fact]
    public void Initial_RoleIsNullAndInverseIsEmpty()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToOne;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        Assert.Equal(default, tram.Get(from, role));
        Assert.Empty(tram.Get(to, inverse));
    }

    [Fact]
    public void Set_PopulatesInverseAndIsIdempotent()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToOne;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Set(from, role, to);
        tram.Set(from, role, to);

        Assert.Equal(to, tram.Get(from, role));
        var inverses = tram.Get(to, inverse).ToArray();
        Assert.Single(inverses);
        Assert.Equal(from, inverses[0]);
    }

    [Fact]
    public void ManyFromsShareOneTo()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToOne;

        var a = tram.Create(this.m.C1);
        var b = tram.Create(this.m.C1);
        var c = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Set(a, role, to);
        tram.Set(b, role, to);
        tram.Set(c, role, to);

        var inverses = tram.Get(to, inverse).ToArray();
        Assert.Equal(3, inverses.Length);
        Assert.Contains(a, inverses);
        Assert.Contains(b, inverses);
        Assert.Contains(c, inverses);
        Assert.Equal(to, tram.Get(a, role));
        Assert.Equal(to, tram.Get(b, role));
        Assert.Equal(to, tram.Get(c, role));
    }

    [Fact]
    public void SetToDefault_ClearsBothEnds()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToOne;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Set(from, role, to);
        tram.Set(from, role, default);

        Assert.Equal(default, tram.Get(from, role));
        Assert.Empty(tram.Get(to, inverse));
    }

    [Fact]
    public void Reassign_RemovesFromOldInverseAndAddsToNew()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToOne;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);
        var toAnother = tram.Create(this.m.C1);

        tram.Set(from, role, to);
        tram.Set(from, role, toAnother);

        Assert.Equal(toAnother, tram.Get(from, role));
        Assert.Empty(tram.Get(to, inverse));
        var inverses = tram.Get(toAnother, inverse).ToArray();
        Assert.Single(inverses);
        Assert.Equal(from, inverses[0]);
    }

    [Fact]
    public void CheckRoleIsAssignable()
    {
        var tram = this.NewTram();
        var m = this.m;

        var c1A = tram.Create(m.C1);
        var c1B = tram.Create(m.C1);
        var c2A = tram.Create(m.C2);

        Assert.Throws<ArgumentException>(() => tram.Set(c1A, m.C1C1C2ManyToOne, c1B));
        Assert.Throws<ArgumentException>(() => tram.Set(c1A, m.C1C1I2ManyToOne, c1B));
        Assert.Throws<ArgumentException>(() => tram.Set(c1A, m.C1C1S2ManyToOne, c1B));
        Assert.Throws<ArgumentException>(() => tram.Set(c1A, m.C3C3C1ManyToOne, c2A));
    }
}
