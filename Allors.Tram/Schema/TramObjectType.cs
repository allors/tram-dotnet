// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A TRAM type whose instances are objects (interfaces and classes).
/// </summary>
public abstract class TramObjectType : TramType
{
    private readonly HashSet<TramInterface> directSupertypes;
    private HashSet<TramInterface>? derivedSupertypes;
    private HashSet<TramObjectType>? derivedSubtypes;

    private HashSet<ITramAttribute>? derivedAttributes;
    private HashSet<ITramProperty>? derivedProperties;
    private HashSet<ITramRelationEnd>? derivedRelationEnds;
    private HashSet<ITramRole>? derivedRoles;
    private HashSet<ITramInverse>? derivedInverses;

    /// <summary>
    /// Creates a new TRAM object type and registers its singular and plural names.
    /// </summary>
    protected TramObjectType(TramSchema schema, string singularName, string pluralName)
        : base(schema, singularName, pluralName)
    {
        this.directSupertypes = [];
    }

    /// <summary>
    /// The interfaces that this type directly inherits from.
    /// </summary>
    public IReadOnlySet<TramInterface> DirectSupertypes
    {
        get => this.directSupertypes;
    }

    /// <summary>
    /// All interfaces that this type inherits from, directly or transitively.
    /// </summary>
    public IReadOnlySet<TramInterface> Supertypes
    {
        get
        {
            if (this.derivedSupertypes != null)
            {
                return this.derivedSupertypes;
            }

            this.derivedSupertypes = [];
            this.AddSupertypes(this.derivedSupertypes);
            return this.derivedSupertypes;
        }
    }

    /// <summary>
    /// All object types that inherit from this type, directly or transitively.
    /// </summary>
    public IReadOnlySet<TramObjectType> Subtypes
    {
        get
        {
            if (this.derivedSubtypes != null)
            {
                return this.derivedSubtypes;
            }

            var classes = this.Schema.TypeByName.Values.OfType<TramObjectType>();
            this.derivedSubtypes = [.. classes.Where(v => v.Supertypes.Contains(this))];
            return this.derivedSubtypes;
        }
    }

    /// <inheritdoc/>
    public override IReadOnlySet<ITramAttribute> Attributes
    {
        get
        {
            if (this.derivedAttributes != null)
            {
                return this.derivedAttributes;
            }

            this.derivedAttributes = new HashSet<ITramAttribute>(this.DeclaredAttributes);

            foreach (var item in this.Supertypes.SelectMany(v => v.DeclaredAttributes))
            {
                this.derivedAttributes.Add(item);
            }

            return this.derivedAttributes;
        }
    }

    /// <inheritdoc/>
    public override IReadOnlySet<ITramProperty> Properties
    {
        get
        {
            if (this.derivedProperties != null)
            {
                return this.derivedProperties;
            }

            this.derivedProperties = [.. this.Attributes.Cast<ITramProperty>().Concat(this.RelationEnds)];
            return this.derivedProperties;
        }
    }

    /// <inheritdoc/>
    public override IReadOnlySet<ITramRelationEnd> RelationEnds
    {
        get
        {
            if (this.derivedRelationEnds != null)
            {
                return this.derivedRelationEnds;
            }

            this.derivedRelationEnds = new HashSet<ITramRelationEnd>(this.DeclaredRelationEnds);

            foreach (var item in this.Supertypes.SelectMany(v => v.DeclaredRelationEnds))
            {
                this.derivedRelationEnds.Add(item);
            }

            return this.derivedRelationEnds;
        }
    }

    /// <inheritdoc/>
    public override IReadOnlySet<ITramRole> Roles
    {
        get
        {
            if (this.derivedRoles != null)
            {
                return this.derivedRoles;
            }

            this.derivedRoles = new HashSet<ITramRole>(this.DeclaredRoles);

            foreach (var item in this.Supertypes.SelectMany(v => v.DeclaredRoles))
            {
                this.derivedRoles.Add(item);
            }

            return this.derivedRoles;
        }
    }

    /// <inheritdoc/>
    public override IReadOnlySet<ITramInverse> Inverses
    {
        get
        {
            if (this.derivedInverses != null)
            {
                return this.derivedInverses;
            }

            this.derivedInverses = new HashSet<ITramInverse>(this.DeclaredInverses);
            foreach (var item in this.Supertypes.SelectMany(v => v.DeclaredInverses))
            {
                this.derivedInverses.Add(item);
            }

            return this.derivedInverses;
        }
    }

    /// <summary>
    /// Adds a direct supertype to this object type.
    /// </summary>
    public void AddDirectSupertype(TramInterface directSupertype)
    {
        this.directSupertypes.Add(directSupertype);
    }

    /// <summary>
    /// Returns true when an object of <paramref name="other"/>'s type can be used wherever this type is expected.
    /// </summary>
    public bool IsAssignableFrom(TramObjectType other)
    {
        return this == other || other.Supertypes.Contains(this);
    }

    internal void AddValueType(TramValueType valueType, string? singularName, string? pluralName)
    {
        singularName ??= valueType.Name;
        pluralName ??= TramSchema.Pluralize(singularName);

        var attribute = new TramAttribute(
            this,
            valueType,
            singularName,
            pluralName);

        attribute.FullName = $"{this.Name}{attribute.Name}";

        this.AddAttribute(attribute);
    }

    internal void AddOneToOne(TramObjectType objectType, string? singularName, string? pluralName)
    {
        singularName ??= objectType.Name;
        pluralName ??= TramSchema.Pluralize(singularName);

        var role = new TramOneToOneRole(
            this,
            objectType,
            singularName,
            pluralName);

        role.Inverse = new TramOneToOneInverse(
            objectType,
            this,
            role,
            role.SingularNameForInverse(this),
            role.PluralNameForInverse(this));

        role.FullName = $"{role.DeclaringType.Name}{role.Name}";

        this.AddRole(role);
        objectType.AddInverse(role.Inverse);
    }

    internal void AddManyToOne(TramObjectType objectType, string? singularName, string? pluralName)
    {
        singularName ??= objectType.Name;
        pluralName ??= TramSchema.Pluralize(singularName);

        var role = new TramManyToOneRole(
            this,
            objectType,
            singularName,
            pluralName);

        role.Inverse = new TramManyToOneInverse(
            objectType,
            this,
            role,
            role.SingularNameForInverse(this),
            role.PluralNameForInverse(this));

        role.FullName = $"{role.DeclaringType.Name}{role.Name}";

        this.AddRole(role);
        objectType.AddInverse(role.Inverse);
    }

    internal void AddOneToMany(TramObjectType objectType, string? singularName, string? pluralName)
    {
        singularName ??= objectType.Name;
        pluralName ??= TramSchema.Pluralize(singularName);

        var role = new TramOneToManyRole(
            this,
            objectType,
            singularName,
            pluralName);

        role.Inverse = new TramOneToManyInverse(
            objectType,
            this,
            role,
            role.SingularNameForInverse(this),
            role.PluralNameForInverse(this));

        role.FullName = $"{role.DeclaringType.Name}{role.Name}";

        this.AddRole(role);
        objectType.AddInverse(role.Inverse);
    }

    internal void AddManyToMany(TramObjectType objectType, string? singularName, string? pluralName)
    {
        singularName ??= objectType.Name;
        pluralName ??= TramSchema.Pluralize(singularName);

        var role = new TramManyToManyRole(
            this,
            objectType,
            singularName,
            pluralName);

        role.Inverse = new TramManyToManyInverse(
            objectType,
            this,
            role,
            role.SingularNameForInverse(this),
            role.PluralNameForInverse(this));

        role.FullName = $"{role.DeclaringType.Name}{role.Name}";

        this.AddRole(role);
        objectType.AddInverse(role.Inverse);
    }

    private void AddSupertypes(HashSet<TramInterface> newDerivedSupertypes)
    {
        foreach (var supertype in this.directSupertypes.Where(supertype => !newDerivedSupertypes.Contains(supertype)))
        {
            newDerivedSupertypes.Add(supertype);
            supertype.AddSupertypes(newDerivedSupertypes);
        }
    }
}
