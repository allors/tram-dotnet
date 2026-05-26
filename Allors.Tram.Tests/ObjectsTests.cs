// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using System;
using System.Linq;
using Allors.Tram.Default;
using Allors.Tram.Schema;
using Xunit;

public class ObjectsTests : TestBase
{
    [Fact]
    public void Filter()
    {
        var tram = this.NewTram();
        var m = this.m;

        Handle NewPerson(string firstNameValue, string lastNameValue)
        {
            var @object = tram.Create(m.Person);
            tram.Set(@object, m.PersonFirstName, firstNameValue);
            tram.Set(@object, m.PersonLastName, lastNameValue);
            return @object;
        }

        var jane = NewPerson("Jane", "Doe");
        var john = NewPerson("John", "Doe");
        var jenny = NewPerson("Jenny", "Doe");

        var lastNameDoe = tram.Objects().Where(v => (string)tram.Get(v, m.PersonLastName)! == "Doe").ToArray();

        Assert.Equal(3, lastNameDoe.Length);
        Assert.Contains(jane, lastNameDoe);
        Assert.Contains(john, lastNameDoe);
        Assert.Contains(jenny, lastNameDoe);

        var fourLetterFirstNames = tram.Objects().Where(v => ((string)tram.Get(v, m.PersonFirstName)!).Length == 4).ToArray();
        Assert.Equal(2, fourLetterFirstNames.Length);
        Assert.Contains(jane, fourLetterFirstNames);
        Assert.Contains(john, fourLetterFirstNames);

        var fiveLetterFirstNames = tram.Objects().Where(v => ((string)tram.Get(v, m.PersonFirstName)!).Length == 5).ToArray();
        Assert.Single(fiveLetterFirstNames);
        Assert.Contains(jenny, fiveLetterFirstNames);
    }

    [Fact]
    public void New_NestedBuilderComposesObjectsAndRelations()
    {
        var tram = this.NewTram();
        var m = this.m;

        var (organizationWhereOwner, owner) = m.OrganizationOwner;

        Handle Build(TramClass @class, Action<Handle> builder)
        {
            var @object = tram.Create(@class);
            builder(@object);
            return @object;
        }

        var acme = Build(
            m.Organization,
            v =>
            {
                tram.Set(v, m.OrganizationName, "Acme");
                tram.Set(v, owner, Build(m.Person, w => tram.Set(w, m.OrganizationName, "Jane")));
            });

        var jane = tram.Get(acme, owner)!;

        Assert.Equal("Acme", tram.Get(acme, m.OrganizationName));
        Assert.Equal(jane, tram.Get(acme, owner));
        Assert.Equal(acme, tram.Get(jane, organizationWhereOwner));
    }

    [Fact]
    public void ObjectsOfType_OmitsDeletedObjects()
    {
        var tram = this.NewTram();
        var m = this.m;

        var acme = tram.Create(m.Organization);
        var john = tram.Create(m.Person);
        var jane = tram.Create(m.Person);

        tram.Delete(jane);

        Assert.Equal([acme], tram.ObjectsOfType(m.Organization).ToArray());
        Assert.Equal([john], tram.ObjectsOfType(m.Person).ToArray());
    }

    [Fact]
    public void SetToOne_UnknownOrDeletedHandle_TreatsAsNull()
    {
        var tram = this.NewTram();
        var m = this.m;

        var (organizationWhereOwner, owner) = m.OrganizationOwner;
        var (organizationWhereAccountant, accountant) = m.OrganizationAccountant;

        var acme = tram.Create(m.Organization);
        var jane = tram.Create(m.Person);

        tram.Set(acme, owner, jane);

        Handle unknown = 999;
        tram.Set(acme, owner, unknown);

        Assert.True(tram.Get(acme, owner).IsNull);
        Assert.True(tram.Get(jane, organizationWhereOwner).IsNull);

        var bob = tram.Create(m.Person);
        tram.Set(acme, accountant, bob);

        var deleted = tram.Create(m.Person);
        tram.Delete(deleted);

        tram.Set(acme, accountant, deleted);

        Assert.True(tram.Get(acme, accountant).IsNull);
        Assert.Empty(tram.Get(bob, organizationWhereAccountant));
    }

    [Fact]
    public void AddRemoveToMany_UnknownOrDeletedHandle_IsIgnored()
    {
        var tram = this.NewTram();
        var m = this.m;

        var (personWhereEmployees, employees) = m.OrganizationEmployees;
        var (organizationsWhereCustomer, customers) = m.OrganizationCustomers;

        var acme = tram.Create(m.Organization);
        var john = tram.Create(m.Person);
        var jane = tram.Create(m.Person);

        tram.Add(acme, employees, john);
        tram.Add(acme, customers, jane);

        Handle unknown = 999;
        var deleted = tram.Create(m.Person);
        tram.Delete(deleted);

        tram.Add(acme, employees, unknown);
        tram.Add(acme, employees, deleted);
        tram.Remove(acme, employees, unknown);
        tram.Remove(acme, employees, deleted);

        Assert.Equal([john], tram.Get(acme, employees).ToArray());
        Assert.Equal(acme, tram.Get(john, personWhereEmployees));

        tram.Add(acme, customers, unknown);
        tram.Add(acme, customers, deleted);
        tram.Remove(acme, customers, unknown);
        tram.Remove(acme, customers, deleted);

        Assert.Equal([jane], tram.Get(acme, customers).ToArray());
        Assert.Contains(acme, tram.Get(jane, organizationsWhereCustomer));
    }

    [Fact]
    public void SetToMany_DeduplicatesAndFiltersDeleted()
    {
        var tram = this.NewTram();
        var (inverse, role) = this.m.C1C1C1ManyToManies;

        var from = tram.Create(this.m.C1);
        var b = tram.Create(this.m.C1);
        var c = tram.Create(this.m.C1);

        tram.Set(from, role, [b, b, c, c, b]);

        Assert.Equal(2, tram.Get(from, role).Count());
        Assert.Contains(b, tram.Get(from, role));
        Assert.Contains(c, tram.Get(from, role));

        tram.Delete(b);

        tram.Set(from, role, [b, c]);

        Assert.Single(tram.Get(from, role));
        Assert.Contains(c, tram.Get(from, role));
        Assert.Contains(from, tram.Get(c, inverse));
    }

    [Fact]
    public void InterfaceHierarchyAssignableForRoles()
    {
        var tram = this.NewTram();
        var m = this.m;

        var c1A = tram.Create(m.C1);
        var c1B = tram.Create(m.C1);

        var (c1sWhereC1I12ManyToOne, c1c1I12ManyToOne) = m.C1C1I12ManyToOne;
        var (c1WhereC1I12OneToMany, c1c1I12OneToManies) = m.C1C1I12OneToManies;

        tram.Set(c1A, c1c1I12ManyToOne, c1B);

        Assert.Equal(c1B, tram.Get(c1A, c1c1I12ManyToOne));
        Assert.Contains(c1A, tram.Get(c1B, c1sWhereC1I12ManyToOne));

        tram.Add(c1A, c1c1I12OneToManies, c1B);

        Assert.Contains(c1B, tram.Get(c1A, c1c1I12OneToManies));
        Assert.Equal(c1A, tram.Get(c1B, c1WhereC1I12OneToMany));
    }

    [Fact]
    public void HandlesEqualsNull_IsFalseEvenWhenHashCodeZero()
    {
        var handles = Handles.Empty;

        Assert.Equal(0, handles.GetHashCode());
        Assert.False(handles.Equals(null));
        Assert.False(handles == null);
    }

    [Fact]
    public void Deduplicator_SameSingleHandleViaDifferentPaths_YieldsReferenceEqual()
    {
        var deduplicator = new Deduplicator();
        var handle = new Handle(42);

        var viaAppend = deduplicator.Append(null, handle);
        var viaFromUnordered = deduplicator.FromUnordered(new[] { handle });

        Assert.Same(viaAppend, viaFromUnordered);
    }
}
