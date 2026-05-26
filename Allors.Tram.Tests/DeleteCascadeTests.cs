// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using System;
using Xunit;

public class DeleteCascadeTests : TestBase
{
    [Fact]
    public void AlreadyDeleted_Throws()
    {
        var tram = this.NewTram();
        var john = tram.Create(this.m.Person);
        tram.Delete(john);

        var exception = Assert.Throws<ArgumentException>(() => tram.Delete(john));
        Assert.Equal("Object is already deleted.", exception.Message);
    }

    [Fact]
    public void ClearsManyToOneInverses()
    {
        var tram = this.NewTram();
        var (_, role) = this.m.C1C1C1ManyToOne;

        var from1 = tram.Create(this.m.C1);
        var from2 = tram.Create(this.m.C1);
        var to = tram.Create(this.m.C1);

        tram.Set(from1, role, to);
        tram.Set(from2, role, to);

        tram.Delete(to);

        Assert.True(tram.Get(from1, role).IsNull);
        Assert.True(tram.Get(from2, role).IsNull);
    }

    [Fact]
    public void ClearsOneToManyInverses()
    {
        var tram = this.NewTram();
        var (inverse, employees) = this.m.OrganizationEmployees;

        var acme = tram.Create(this.m.Organization);
        var john = tram.Create(this.m.Person);
        var jane = tram.Create(this.m.Person);

        tram.Set(acme, employees, [john, jane]);

        tram.Delete(acme);

        Assert.True(tram.Get(john, inverse).IsNull);
        Assert.True(tram.Get(jane, inverse).IsNull);
        Assert.True(tram.Exists(john));
        Assert.True(tram.Exists(jane));
    }

    [Fact]
    public void ClearsManyToManyInverses()
    {
        var tram = this.NewTram();
        var (inverse, customers) = this.m.OrganizationCustomers;

        var acme = tram.Create(this.m.Organization);
        var john = tram.Create(this.m.Person);
        var jane = tram.Create(this.m.Person);

        tram.Set(acme, customers, [john, jane]);

        tram.Delete(acme);

        Assert.Empty(tram.Get(john, inverse));
        Assert.Empty(tram.Get(jane, inverse));
        Assert.True(tram.Exists(john));
        Assert.True(tram.Exists(jane));
    }

    [Fact]
    public void DeletesMiddleOfBidirectionalChain_DetachesNeighbours()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1OneToOne;

        var a = tram.Create(this.m.C1);
        var b = tram.Create(this.m.C1);
        var c = tram.Create(this.m.C1);

        tram.Set(a, role, b);
        tram.Set(b, role, c);

        tram.Delete(b);

        Assert.True(tram.Get(a, role).IsNull);
        Assert.True(tram.Get(c, inverse).IsNull);
        Assert.True(tram.Exists(a));
        Assert.True(tram.Exists(c));
    }

    [Fact]
    public void SelfReferentialOneToOne_IsDeleted()
    {
        var tram = this.NewTram();
        var (_, role) = this.m.C1C1C1OneToOne;

        var a = tram.Create(this.m.C1);
        tram.Set(a, role, a);

        tram.Delete(a);

        Assert.False(tram.Exists(a));
    }

    [Fact]
    public void SelfReferentialOneToMany_IsDeleted()
    {
        var tram = this.NewTram();
        var (_, role) = this.m.C1C1C1OneToManies;

        var a = tram.Create(this.m.C1);
        tram.Add(a, role, a);

        tram.Delete(a);

        Assert.False(tram.Exists(a));
    }

    [Fact]
    public void SelfReferentialManyToOne_IsDeleted()
    {
        var tram = this.NewTram();
        var (_, role) = this.m.C1C1C1ManyToOne;

        var a = tram.Create(this.m.C1);
        tram.Set(a, role, a);

        tram.Delete(a);

        Assert.False(tram.Exists(a));
    }

    [Fact]
    public void SelfReferentialManyToMany_IsDeleted()
    {
        var tram = this.NewTram();
        var (_, role) = this.m.C1C1C1ManyToManies;

        var a = tram.Create(this.m.C1);
        tram.Add(a, role, a);

        tram.Delete(a);

        Assert.False(tram.Exists(a));
    }

    [Fact]
    public void ParticipatingInAllMultiplicityTypes_CleansAllInverses()
    {
        var tram = this.NewTram();
        var (oneToOneInverse, oneToOneRole) = this.m.C1C1C1OneToOne;
        var (oneToManyInverse, oneToManyRole) = this.m.C1C1C1OneToManies;
        var (_, manyToOneRole) = this.m.C1C1C1ManyToOne;
        var (manyToManyInverse, manyToManyRole) = this.m.C1C1C1ManyToManies;

        var a = tram.Create(this.m.C1);
        var b = tram.Create(this.m.C1);
        var c = tram.Create(this.m.C1);
        var d = tram.Create(this.m.C1);
        var e = tram.Create(this.m.C1);

        tram.Set(a, oneToOneRole, b);
        tram.Add(a, oneToManyRole, c);
        tram.Set(d, manyToOneRole, a);
        tram.Set(a, manyToManyRole, [e]);

        tram.Delete(a);

        Assert.True(tram.Get(b, oneToOneInverse).IsNull);
        Assert.True(tram.Get(c, oneToManyInverse).IsNull);
        Assert.True(tram.Get(d, manyToOneRole).IsNull);
        Assert.Empty(tram.Get(e, manyToManyInverse));

        Assert.True(tram.Exists(b));
        Assert.True(tram.Exists(c));
        Assert.True(tram.Exists(d));
        Assert.True(tram.Exists(e));
    }
}
