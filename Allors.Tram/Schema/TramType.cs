// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Base class for every type in a TRAM schema (value types and object types).
/// </summary>
public abstract class TramType
{
    private readonly HashSet<ITramAttribute> declaredAttributes;
    private readonly HashSet<ITramRole> declaredRoles;
    private readonly HashSet<ITramInverse> declaredInverses;

    private HashSet<ITramProperty>? derivedDeclaredProperties;
    private HashSet<ITramRelationEnd>? derivedDeclaredRelationEnds;
    private Dictionary<string, ITramProperty>? derivedPropertyBySingularOrPluralName;

    /// <summary>
    /// Creates a new TRAM type and registers its singular and plural names.
    /// </summary>
    protected TramType(TramSchema schema, string singularName, string pluralName)
    {
        this.Schema = schema;
        this.SingularName = singularName;
        this.PluralName = pluralName;
        this.declaredAttributes = [];
        this.declaredRoles = [];
        this.declaredInverses = [];
    }

    /// <summary>
    /// The schema this type belongs to.
    /// </summary>
    public TramSchema Schema { get; }

    /// <summary>
    /// The default name of this type (the singular name).
    /// </summary>
    public string Name => this.SingularName;

    /// <summary>
    /// The singular form of this type's name.
    /// </summary>
    public string SingularName { get; }

    /// <summary>
    /// The plural form of this type's name.
    /// </summary>
    public string PluralName { get; }

    /// <summary>
    /// The properties declared directly on this type, excluding inherited properties.
    /// </summary>
    public IReadOnlySet<ITramProperty> DeclaredProperties
    {
        get
        {
            if (this.derivedDeclaredProperties != null)
            {
                return this.derivedDeclaredProperties;
            }

            this.derivedDeclaredProperties = [.. this.DeclaredAttributes.Cast<ITramProperty>().Concat(this.DeclaredRelationEnds)];
            return this.derivedDeclaredProperties;
        }
    }

    /// <summary>
    /// The attributes declared directly on this type, excluding inherited attributes.
    /// </summary>
    public IReadOnlySet<ITramAttribute> DeclaredAttributes
    {
        get => this.declaredAttributes;
    }

    /// <summary>
    /// The attributes declared on this type and inherited from any supertype.
    /// </summary>
    public abstract IReadOnlySet<ITramAttribute> Attributes { get; }

    /// <summary>
    /// The relation ends declared directly on this type, excluding inherited relation ends.
    /// </summary>
    public IReadOnlySet<ITramRelationEnd> DeclaredRelationEnds
    {
        get
        {
            if (this.derivedDeclaredRelationEnds != null)
            {
                return this.derivedDeclaredRelationEnds;
            }

            this.derivedDeclaredRelationEnds = [.. this.DeclaredRoles.Cast<ITramRelationEnd>().Concat(this.DeclaredInverses)];
            return this.derivedDeclaredRelationEnds;
        }
    }

    /// <summary>
    /// The relation ends declared on this type and inherited from any supertype.
    /// </summary>
    public abstract IReadOnlySet<ITramRelationEnd> RelationEnds { get; }

    /// <summary>
    /// The roles declared directly on this type, excluding inherited roles.
    /// </summary>
    public IReadOnlySet<ITramRole> DeclaredRoles
    {
        get => this.declaredRoles;
    }

    /// <summary>
    /// The roles declared on this type and inherited from any supertype.
    /// </summary>
    public abstract IReadOnlySet<ITramRole> Roles { get; }

    /// <summary>
    /// The inverses declared directly on this type, excluding inherited inverses.
    /// </summary>
    public IReadOnlySet<ITramInverse> DeclaredInverses
    {
        get => this.declaredInverses;
    }

    /// <summary>
    /// The inverses declared on this type and inherited from any supertype.
    /// </summary>
    public abstract IReadOnlySet<ITramInverse> Inverses { get; }

    /// <summary>
    /// All properties (attributes and relation ends) declared on this type and inherited from any supertype.
    /// </summary>
    public abstract IReadOnlySet<ITramProperty> Properties { get; }

    /// <summary>
    /// Properties of this type indexed by both their singular and plural names.
    /// </summary>
    public IDictionary<string, ITramProperty> PropertyBySingularOrPluralName
    {
        get
        {
            if (this.derivedPropertyBySingularOrPluralName != null)
            {
                return this.derivedPropertyBySingularOrPluralName;
            }

            this.derivedPropertyBySingularOrPluralName = new Dictionary<string, ITramProperty>();
            foreach (var item in this.Properties)
            {
                this.derivedPropertyBySingularOrPluralName.Add(item.SingularName, item);
                this.derivedPropertyBySingularOrPluralName.Add(item.PluralName, item);
            }

            return this.derivedPropertyBySingularOrPluralName;
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.Name;
    }

    internal void AddAttribute(ITramAttribute attribute)
    {
        this.declaredAttributes.Add(attribute);
    }

    internal void AddRole(ITramRole role)
    {
        this.declaredRoles.Add(role);
    }

    internal void AddInverse(ITramInverse inverse)
    {
        this.declaredInverses.Add(inverse);
    }
}
