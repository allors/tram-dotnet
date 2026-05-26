// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema.Tests;

using Allors.Tram.Schema.Config;
using Allors.Tram.Schema.Diagrams;
using Xunit;

public class MermaidClassDiagramTests
{
    [Fact]
    public void Inheritance()
    {
        var config = new TramSchemaConfigBuilder()
            .AddInterface("S1")
            .AddInterface("I1", ["S1"])
            .AddClass("C1", ["I1"])
            .Build();
        var schema = new TramSchema(config);

        var diagram = schema.ToMermaidClassDiagram();

        Assert.Equal(
            @"classDiagram
    class C1
    I1 <|-- C1
    class I1
    S1 <|-- I1
    class S1
",
            diagram);
    }

    [Fact]
    public void RelationEnds()
    {
        var config = new TramSchemaConfigBuilder()
            .AddClass("Organization")
            .AddClass("Person")
            .AddOneToMany("Organization", "Person", "Employee")
            .Build();
        var schema = new TramSchema(config);

        var diagram = schema.ToMermaidClassDiagram();

        Assert.Equal(
            """
            classDiagram
                class Organization
                Organization o-- Person : Employees
                class Person

            """,
            diagram);
    }

    [Fact]
    public void InheritedRoles()
    {
        var config = new TramSchemaConfigBuilder()
            .AddInterface("InternalOrganization")
            .AddClass("Organization", ["InternalOrganization"])
            .AddClass("Person")
            .AddOneToMany("InternalOrganization", "Person", "Employee")
            .AddOneToMany("Organization", "Person", "Customer")
            .Build();
        var schema = new TramSchema(config);

        var diagram = schema.ToMermaidClassDiagram();

        Assert.Equal(
            """
            classDiagram
                class InternalOrganization
                InternalOrganization o-- Person : Employees
                class Organization
                InternalOrganization <|-- Organization
                Organization o-- Person : Customers
                class Person

            """,
            diagram);
    }

    [Fact]
    public void Title()
    {
        var schema = new TramSchema(new TramSchemaConfigBuilder().Build());

        var options = new MermaidClassDiagramOptions { Title = "My Empty ClassDiagram" };
        var diagram = schema.ToMermaidClassDiagram(options);

        Assert.Equal(
            """
            ---
            title: "My Empty ClassDiagram"
            ---
            classDiagram

            """,
            diagram);
    }

    [Fact]
    public void TitleWithYamlSensitiveCharacters()
    {
        var schema = new TramSchema(new TramSchemaConfigBuilder().Build());

        var options = new MermaidClassDiagramOptions { Title = "Schema: Sales" };
        var diagram = schema.ToMermaidClassDiagram(options);

        Assert.Equal(
            """
            ---
            title: "Schema: Sales"
            ---
            classDiagram

            """,
            diagram);
    }

    [Fact]
    public void TitleWithEscapableCharacters()
    {
        var schema = new TramSchema(new TramSchemaConfigBuilder().Build());

        var options = new MermaidClassDiagramOptions { Title = "a\"b\\c\nd" };
        var diagram = schema.ToMermaidClassDiagram(options);

        Assert.Equal(
            "---\ntitle: \"a\\\"b\\\\c\\nd\"\n---\nclassDiagram\n",
            diagram);
    }

    [Fact]
    public void Multiplicity()
    {
        var schemaConfig = new TramSchemaConfigBuilder()
            .AddClass("Organization")
            .AddClass("Person")
            .AddOneToMany("Organization", "Person", "Employee")
            .Build();
        var schema = new TramSchema(schemaConfig);

        var options = new MermaidClassDiagramOptions { OneMultiplicity = "1", ManyMultiplicity = "1..*" };
        var diagram = schema.ToMermaidClassDiagram(options);

        Assert.Equal(
            """
            classDiagram
                class Organization
                Organization "1" o-- "1..*" Person : Employees
                class Person

            """,
            diagram);
    }

    [Fact]
    public void MultiplicityOne()
    {
        var schemaConfig = new TramSchemaConfigBuilder()
            .AddClass("Organization")
            .AddClass("Person")
            .AddOneToMany("Organization", "Person", "Employee")
            .Build();
        var schema = new TramSchema(schemaConfig);

        var options = new MermaidClassDiagramOptions { OneMultiplicity = "one" };
        var diagram = schema.ToMermaidClassDiagram(options);

        Assert.Equal(
            """
            classDiagram
                class Organization
                Organization "one" o-- Person : Employees
                class Person

            """,
            diagram);
    }

    [Fact]
    public void MultiplicityMany()
    {
        var schemaConfig = new TramSchemaConfigBuilder()
            .AddClass("Organization")
            .AddClass("Person")
            .AddOneToMany("Organization", "Person", "Employee")
            .Build();
        var schema = new TramSchema(schemaConfig);

        var options = new MermaidClassDiagramOptions { ManyMultiplicity = "many" };
        var diagram = schema.ToMermaidClassDiagram(options);

        Assert.Equal(
            """
            classDiagram
                class Organization
                Organization o-- "many" Person : Employees
                class Person

            """,
            diagram);
    }
}
