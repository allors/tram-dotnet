// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema.Config;

using System.Collections.Generic;

/// <summary>
/// Fluent builder for <see cref="TramSchemaConfig"/>.
/// </summary>
public class TramSchemaConfigBuilder
{
    private readonly List<TramTypeConfig> types;
    private readonly List<TramAttributeConfig> attributes;
    private readonly List<TramRelationConfig> relations;

    /// <summary>
    /// Creates a new, empty builder.
    /// </summary>
    public TramSchemaConfigBuilder()
    {
        this.types = [];
        this.attributes = [];
        this.relations = [];
    }

    /// <summary>
    /// Creates a new builder pre-populated from the given schema configuration.
    /// </summary>
    public TramSchemaConfigBuilder(TramSchemaConfig template)
    {
        this.types = [..template.Types];
        this.attributes = [..template.Attributes];
        this.relations = [..template.Relations];
    }

    /// <summary>
    /// Creates a new builder pre-populated from another builder's current contents.
    /// </summary>
    public TramSchemaConfigBuilder(TramSchemaConfigBuilder templateBuilder)
        : this(templateBuilder.Build())
    {
    }

    /// <summary>
    /// Adds a value type with the given singular name.
    /// </summary>
    public TramSchemaConfigBuilder AddValueType(string singularName)
    {
        var typeConfig = new TramTypeConfig { Kind = TramTypeKind.ValueType, SingularName = singularName };
        return this.AddType(typeConfig);
    }

    /// <summary>
    /// Adds an interface type with the given singular name and optional direct supertypes.
    /// </summary>
    public TramSchemaConfigBuilder AddInterface(string singularName, string[]? directSupertypes = null)
    {
        var typeConfig = new TramTypeConfig { Kind = TramTypeKind.Interface, SingularName = singularName, DirectSupertypes = directSupertypes };
        return this.AddType(typeConfig);
    }

    /// <summary>
    /// Adds an interface type with the given singular and plural names and optional direct supertypes.
    /// </summary>
    public TramSchemaConfigBuilder AddInterface(string singularName, string pluralName, string[]? directSupertypes = null)
    {
        var typeConfig = new TramTypeConfig
        {
            Kind = TramTypeKind.Interface, SingularName = singularName, PluralName = pluralName, DirectSupertypes = directSupertypes,
        };
        return this.AddType(typeConfig);
    }

    /// <summary>
    /// Adds a class type with the given singular name and optional direct supertypes.
    /// </summary>
    public TramSchemaConfigBuilder AddClass(string singularName, string[]? directSupertypes = null)
    {
        var typeConfig = new TramTypeConfig { Kind = TramTypeKind.Class, SingularName = singularName, DirectSupertypes = directSupertypes };
        return this.AddType(typeConfig);
    }

    /// <summary>
    /// Adds a class type with the given singular and plural names and optional direct supertypes.
    /// </summary>
    public TramSchemaConfigBuilder AddClass(string singularName, string pluralName, string[]? directSupertypes = null)
    {
        var typeConfig = new TramTypeConfig
        {
            Kind = TramTypeKind.Class, SingularName = singularName, PluralName = pluralName, DirectSupertypes = directSupertypes,
        };
        return this.AddType(typeConfig);
    }

    /// <summary>
    /// Adds the given type configuration.
    /// </summary>
    public TramSchemaConfigBuilder AddType(TramTypeConfig typeConfig)
    {
        this.types.Add(typeConfig);
        return this;
    }

    /// <summary>
    /// Adds an attribute with the given declaring type, value type and singular name.
    /// </summary>
    public TramSchemaConfigBuilder AddAttribute(string declaringType, string type, string singularName)
    {
        return this.AddAttribute(new TramAttributeConfig { DeclaringType = declaringType, Type = type, SingularName = singularName });
    }

    /// <summary>
    /// Adds the given attribute configuration.
    /// </summary>
    public TramSchemaConfigBuilder AddAttribute(TramAttributeConfig attributeConfig)
    {
        this.attributes.Add(attributeConfig);
        return this;
    }

    /// <summary>
    /// Adds a one-to-one relationship between two object types.
    /// </summary>
    public TramSchemaConfigBuilder AddOneToOne(string declaringType, string type, string? singularName = null, string? pluralName = null)
    {
        return this.AddRelation(
            new TramRelationConfig
            {
                DeclaringType = declaringType,
                Type = type,
                SingularName = singularName,
                PluralName = pluralName,
                Multiplicity = TramMultiplicity.OneToOne,
            });
    }

    /// <summary>
    /// Adds a one-to-many relationship between two object types.
    /// </summary>
    public TramSchemaConfigBuilder AddOneToMany(string declaringType, string type, string? singularName = null, string? pluralName = null)
    {
        return this.AddRelation(
            new TramRelationConfig
            {
                DeclaringType = declaringType,
                Type = type,
                SingularName = singularName,
                PluralName = pluralName,
                Multiplicity = TramMultiplicity.OneToMany,
            });
    }

    /// <summary>
    /// Adds a many-to-one relationship between two object types.
    /// </summary>
    public TramSchemaConfigBuilder AddManyToOne(string declaringType, string type, string? singularName = null, string? pluralName = null)
    {
        return this.AddRelation(
            new TramRelationConfig
            {
                DeclaringType = declaringType,
                Type = type,
                SingularName = singularName,
                PluralName = pluralName,
                Multiplicity = TramMultiplicity.ManyToOne,
            });
    }

    /// <summary>
    /// Adds a many-to-many relationship between two object types.
    /// </summary>
    public TramSchemaConfigBuilder AddManyToMany(string declaringType, string type, string? singularName = null, string? pluralName = null)
    {
        return this.AddRelation(
            new TramRelationConfig
            {
                DeclaringType = declaringType,
                Type = type,
                SingularName = singularName,
                PluralName = pluralName,
                Multiplicity = TramMultiplicity.ManyToMany,
            });
    }

    /// <summary>
    /// Adds the given relation configuration.
    /// </summary>
    public TramSchemaConfigBuilder AddRelation(TramRelationConfig relationConfig)
    {
        this.relations.Add(relationConfig);
        return this;
    }

    /// <summary>
    /// Builds and returns a <see cref="TramSchemaConfig"/> from the current contents of this builder.
    /// </summary>
    public TramSchemaConfig Build()
    {
        return new TramSchemaConfig { Types = [..this.types], Attributes = [..this.attributes], Relations = [..this.relations] };
    }
}
