// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using Xunit;

public class ValueTests : TestBase
{
    [Fact]
    public void SamePropertyNameAcrossTypes()
    {
        var tram = this.NewTram();
        var m = this.m;

        var c1 = m.C1;
        var c2 = m.C2;
        var c1Same = m.C1Same;
        var c2Same = m.C2Same;

        var c1A = tram.Create(c1);
        tram.Set(c1A, c1Same, "c1");

        var c2A = tram.Create(c2);
        tram.Set(c2A, c2Same, "c2");

        Assert.Equal("c1", tram.Get(c1A, c1Same));
        Assert.Equal("c2", tram.Get(c2A, c2Same));
    }

    [Fact]
    public void PropertySet()
    {
        var tram = this.NewTram();
        var m = this.m;

        var person = m.Person;
        var firstName = m.PersonFirstName;

        var john = tram.Create(person);
        var jane = tram.Create(person);

        tram.Set(john, firstName, "John");
        tram.Set(jane, firstName, "Jane");

        Assert.Equal("John", tram.Get(john, firstName));
        Assert.Equal("Jane", tram.Get(jane, firstName));
        Assert.Equal("John", tram.Get(john, firstName));
        Assert.Equal("Jane", tram.Get(jane, firstName));

        tram.Set(jane, firstName, null);

        Assert.Equal("John", tram.Get(john, firstName));
        Assert.Null(tram.Get(jane, firstName));
        Assert.Equal("John", tram.Get(john, firstName));
        Assert.Null(tram.Get(jane, firstName));
    }
}
