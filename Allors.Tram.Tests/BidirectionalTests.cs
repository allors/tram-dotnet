// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using System.Linq;
using Xunit;

public class BidirectionalTests : TestBase
{
    [Fact]
    public void OneToOne_StealOwnerFromPrevious()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToOne;

        var from = tram.Create(this.m.C1);
        var fromAnother = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Set(from, role, to);
        tram.Set(fromAnother, role, to);

        Assert.Equal(fromAnother, tram.Get(to, inverse));
        Assert.Equal(default, tram.Get(from, role));
        Assert.Equal(to, tram.Get(fromAnother, role));
    }

    [Fact]
    public void OneToOne_DisplaceTargetByReassigning()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToOne;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);
        var toAnother = tram.Create(this.m.C1);

        tram.Set(from, role, to);
        tram.Set(from, role, toAnother);

        Assert.Equal(default, tram.Get(to, inverse));
        Assert.Equal(from, tram.Get(toAnother, inverse));
        Assert.Equal(toAnother, tram.Get(from, role));
    }

    [Fact]
    public void OneToOne_ChainDisplacesMiddle()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToOne;

        var begin = tram.Create(this.m.C1);
        var middle = tram.Create(this.m.C1);
        var end = tram.Create(this.m.C1);

        tram.Set(begin, role, middle);
        tram.Set(middle, role, end);
        tram.Set(begin, role, end);

        Assert.Equal(default, tram.Get(middle, inverse));
        Assert.Equal(begin, tram.Get(end, inverse));
        Assert.Equal(default, tram.Get(begin, inverse));

        Assert.Equal(end, tram.Get(begin, role));
        Assert.Equal(default, tram.Get(middle, role));
        Assert.Equal(default, tram.Get(end, role));
    }

    [Fact]
    public void OneToOne_FormsRingOfThree()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToOne;

        var begin = tram.Create(this.m.C1);
        var middle = tram.Create(this.m.C1);
        var end = tram.Create(this.m.C1);

        tram.Set(begin, role, middle);
        tram.Set(middle, role, end);
        tram.Set(end, role, begin);

        Assert.Equal(begin, tram.Get(middle, inverse));
        Assert.Equal(middle, tram.Get(end, inverse));
        Assert.Equal(end, tram.Get(begin, inverse));

        Assert.Equal(middle, tram.Get(begin, role));
        Assert.Equal(end, tram.Get(middle, role));
        Assert.Equal(begin, tram.Get(end, role));
    }

    [Fact]
    public void OneToMany_ChildReassignmentMovesBetweenOwners()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToManies;

        var from = tram.Create(this.m.C1);
        var fromAnother = tram.Create(this.m.C1);
        var child = tram.Create(this.m.C1);

        tram.Add(from, role, child);
        tram.Add(fromAnother, role, child);

        Assert.Empty(tram.Get(from, role));
        Assert.Single(tram.Get(fromAnother, role));
        Assert.Contains(child, tram.Get(fromAnother, role));
        Assert.Equal(fromAnother, tram.Get(child, inverse));
    }

    [Fact]
    public void OneToMany_ChainKeepsMiddleAsChildButEmptiesItsCollection()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToManies;

        var from = tram.Create(this.m.C1);
        var middle = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Add(from, role, middle);
        tram.Add(middle, role, to);
        tram.Add(from, role, to);

        Assert.Equal(from, tram.Get(middle, inverse));
        Assert.Empty(tram.Get(middle, role));
        Assert.Equal(from, tram.Get(to, inverse));
    }

    [Fact]
    public void ManyToOne_StealFromBetweenTargets()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToOne;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);
        var toAnother = tram.Create(this.m.C1);

        tram.Set(from, role, to);
        tram.Set(from, role, toAnother);

        Assert.Empty(tram.Get(to, inverse));
        Assert.Equal([from], tram.Get(toAnother, inverse).ToArray());
        Assert.Equal(toAnother, tram.Get(from, role));
    }

    [Fact]
    public void ManyToMany_RemoveAffectsOnlyOneAssociation()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToManies;

        var from = tram.Create(this.m.C1);
        var fromAnother = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Add(from, role, to);
        tram.Add(fromAnother, role, to);
        tram.Remove(from, role, to);

        Assert.Empty(tram.Get(from, role));
        Assert.Equal([to], tram.Get(fromAnother, role).ToArray());
        Assert.Equal([fromAnother], tram.Get(to, inverse).ToArray());
    }

    [Fact]
    public void SelfReferential_ManyToOne_SetAndClear()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToOne;

        var a = tram.Create(this.m.C1);
        tram.Set(a, role, a);

        Assert.Equal(a, tram.Get(a, role));
        Assert.Contains(a, tram.Get(a, inverse));

        tram.Set(a, role, default);

        Assert.True(tram.Get(a, role).IsNull);
        Assert.Empty(tram.Get(a, inverse));
    }

    [Fact]
    public void SelfReferential_ManyToMany_AddAndRemove()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToManies;

        var a = tram.Create(this.m.C1);
        tram.Add(a, role, a);

        Assert.Contains(a, tram.Get(a, role));
        Assert.Contains(a, tram.Get(a, inverse));

        tram.Remove(a, role, a);

        Assert.Empty(tram.Get(a, role));
        Assert.Empty(tram.Get(a, inverse));
    }
}
