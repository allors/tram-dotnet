// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using Xunit;

public class SandboxTests : TestBase
{
    [Fact]
    public void Test()
    {
        var tram = this.NewTram();
        var m = this.m;

        var (inverse, role) = m.C1C1C1OneToOne;

        var from = tram.Create(m.C1);
        var to = tram.Create(m.C1);
        var toAnother = tram.Create(m.C1);

        tram.Set(from, role, to);

        tram.Derive();

        tram.Set(from, role, toAnother);

        tram.Derive();

        Assert.Equal(tram.Get(to, inverse), default);
    }
}
