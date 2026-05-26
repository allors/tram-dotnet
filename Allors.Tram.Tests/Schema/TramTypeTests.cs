// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema.Tests;

using System;
using System.Collections.Generic;
using Allors.Tram.Schema;
using Allors.Tram.Schema.Config;
using Xunit;

public class TramTypeTests
{
    [Fact]
    public void Supertypes()
    {
        var config = new TramSchemaConfigBuilder()
            .AddInterface("S1")
            .AddInterface("I1", ["S1"])
            .AddClass("C1", ["I1"])
            .Build();
        var schema = new TramSchema(config);

        var c1 = (TramClass)schema.TypeByName["C1"];
        var i1 = (TramInterface)schema.TypeByName["I1"];
        var s1 = (TramInterface)schema.TypeByName["S1"];

        Assert.Equal(2, c1.Supertypes.Count);
        Assert.Contains(i1, c1.Supertypes);
        Assert.Contains(s1, c1.Supertypes);

        Assert.Single(i1.Supertypes);
        Assert.Contains(s1, i1.Supertypes);

        Assert.Empty(s1.Supertypes);
    }

    [Fact]
    public void IsAssignableFrom()
    {
        var config = new TramSchemaConfigBuilder()
            .AddInterface("S1")
            .AddInterface("I1", ["S1"])
            .AddClass("C1", ["I1"])
            .Build();
        var schema = new TramSchema(config);

        var c1 = (TramClass)schema.TypeByName["C1"];
        var i1 = (TramInterface)schema.TypeByName["I1"];
        var s1 = (TramInterface)schema.TypeByName["S1"];

        Assert.True(c1.IsAssignableFrom(c1));
        Assert.True(i1.IsAssignableFrom(c1));
        Assert.True(s1.IsAssignableFrom(c1));

        Assert.False(c1.IsAssignableFrom(i1));
        Assert.True(i1.IsAssignableFrom(i1));
        Assert.True(s1.IsAssignableFrom(i1));

        Assert.False(c1.IsAssignableFrom(s1));
        Assert.False(i1.IsAssignableFrom(s1));
        Assert.True(s1.IsAssignableFrom(s1));
    }

    [Fact]
    public void CircularSupertypeHandledGracefully()
    {
        // Circular supertypes are handled gracefully (no infinite loop)
        // because AddSupertypes checks if supertype is already in the set
        var config = new TramSchemaConfigBuilder()
            .AddInterface("I1", ["I2"])
            .AddInterface("I2", ["I1"])
            .Build();
        var schema = new TramSchema(config);

        var i1 = (TramInterface)schema.TypeByName["I1"];
        var i2 = (TramInterface)schema.TypeByName["I2"];

        // Both should have each other as supertypes (circular)
        Assert.Contains(i2, i1.Supertypes);
        Assert.Contains(i1, i2.Supertypes);
    }

    [Fact]
    public void InvalidInverseReferenceThrows()
    {
        var config = new TramSchemaConfigBuilder()
            .AddInterface("I1")
            .AddOneToOne("NonExistent", "I1")
            .Build();

        var exception = Assert.Throws<KeyNotFoundException>(() => new TramSchema(config));
        Assert.Contains("NonExistent", exception.Message);
    }

    [Fact]
    public void InvalidRoleReferenceThrows()
    {
        var config = new TramSchemaConfigBuilder()
            .AddInterface("I1")
            .AddOneToOne("I1", "NonExistent")
            .Build();

        var exception = Assert.Throws<KeyNotFoundException>(() => new TramSchema(config));
        Assert.Contains("NonExistent", exception.Message);
    }

    [Fact]
    public void ValueTypeAsRelationRoleThrows()
    {
        var config = new TramSchemaConfigBuilder()
            .AddClass("C1")
            .AddValueType("String")
            .AddRelation(new TramRelationConfig { DeclaringType = "C1", Type = "String" })
            .Build();

        Assert.Throws<InvalidCastException>(() => new TramSchema(config));
    }

    [Fact]
    public void ClassExtendingClassThrows()
    {
        var config = new TramSchemaConfigBuilder()
            .AddClass("C1")
            .AddClass("C2", ["C1"])
            .Build();

        // Currently throws InvalidCastException when casting Class to Interface
        Assert.Throws<InvalidCastException>(() => new TramSchema(config));
    }

    [Fact]
    public void PropertiesAndRolesExposeTheSymmetricApi()
    {
        var config = new TramSchemaConfigBuilder()
            .AddValueType("String")
            .AddClass("Organization")
            .AddClass("Person")
            .AddAttribute("Organization", "String", "Name")
            .AddOneToOne("Organization", "Person", "Owner")
            .Build();
        var schema = new TramSchema(config);

        var organization = (TramClass)schema.TypeByName["Organization"];
        var person = (TramClass)schema.TypeByName["Person"];
        var name = (TramAttribute)organization.PropertyBySingularOrPluralName["Name"];
        var owner = (TramOneToOneRole)organization.PropertyBySingularOrPluralName["Owner"];
        var organizationWhereOwner = (TramOneToOneInverse)person.PropertyBySingularOrPluralName["OrganizationWhereOwner"];

        Assert.Equal(organization, name.DeclaringType);
        Assert.Equal(schema.TypeByName["String"], name.Type);

        Assert.IsAssignableFrom<ITramRelationEnd>(owner);
        Assert.IsAssignableFrom<ITramRole>(owner);
        Assert.IsAssignableFrom<ITramToOneRelationEnd>(owner);
        Assert.IsAssignableFrom<ITramToOneRole>(owner);
        Assert.Equal(organization, owner.DeclaringType);
        Assert.Equal(person, owner.Type);
        Assert.Equal(organizationWhereOwner, owner.OtherEnd);
        Assert.Equal(organizationWhereOwner, owner.Inverse);

        Assert.IsAssignableFrom<ITramRelationEnd>(organizationWhereOwner);
        Assert.IsAssignableFrom<ITramInverse>(organizationWhereOwner);
        Assert.IsAssignableFrom<ITramToOneRelationEnd>(organizationWhereOwner);
        Assert.IsAssignableFrom<ITramToOneInverse>(organizationWhereOwner);
        Assert.Equal(person, organizationWhereOwner.DeclaringType);
        Assert.Equal(organization, organizationWhereOwner.Type);
        Assert.Equal(owner, organizationWhereOwner.OtherEnd);
        Assert.Equal(owner, organizationWhereOwner.Role);
    }
}
