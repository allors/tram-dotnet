// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using Xunit;

public class OneToOneTests : TestBase
{
    [Fact]
    public void Initial_RoleAndInverseAreNull()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToOne;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        Assert.Equal(default, tram.Get(from, role));
        Assert.Equal(default, tram.Get(to, inverse));
    }

    [Fact]
    public void SetRole_PopulatesInverse()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToOne;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Set(from, role, to);

        Assert.Equal(to, tram.Get(from, role));
        Assert.Equal(from, tram.Get(to, inverse));
    }

    [Fact]
    public void SetRoleThenNull_ClearsBothEnds()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToOne;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Set(from, role, to);
        tram.Set(from, role, default);

        Assert.Equal(default, tram.Get(from, role));
        Assert.Equal(default, tram.Get(to, inverse));
    }

    [Fact]
    public void HeterogeneousPair_SetRolePopulatesInverse()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C2OneToOne;

        var from = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C2);

        tram.Set(from, role, to);

        Assert.Equal(to, tram.Get(from, role));
        Assert.Equal(from, tram.Get(to, inverse));
    }
}
