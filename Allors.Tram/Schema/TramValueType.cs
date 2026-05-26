// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

using System.Collections.Generic;

/// <summary>
/// A TRAM type that represents a value (the type of an attribute). Has no properties, supertypes or subtypes.
/// </summary>
public sealed class TramValueType : TramType
{
    private static readonly HashSet<ITramAttribute> EmptyAttributes = [];
    private static readonly HashSet<ITramProperty> EmptyProperties = [];
    private static readonly HashSet<ITramRelationEnd> EmptyRelationEnds = [];
    private static readonly HashSet<ITramRole> EmptyRoles = [];
    private static readonly HashSet<ITramInverse> EmptyInverses = [];

    /// <summary>
    /// Creates a new <see cref="TramValueType"/> with the given singular and plural names.
    /// </summary>
    public TramValueType(TramSchema schema, string singularName, string pluralName)
        : base(schema, singularName, pluralName)
    {
    }

    /// <inheritdoc/>
    public override IReadOnlySet<ITramAttribute> Attributes
    {
        get => EmptyAttributes;
    }

    /// <inheritdoc/>
    public override IReadOnlySet<ITramProperty> Properties
    {
        get => EmptyProperties;
    }

    /// <inheritdoc/>
    public override IReadOnlySet<ITramRelationEnd> RelationEnds
    {
        get => EmptyRelationEnds;
    }

    /// <inheritdoc/>
    public override IReadOnlySet<ITramRole> Roles
    {
        get => EmptyRoles;
    }

    /// <inheritdoc/>
    public override IReadOnlySet<ITramInverse> Inverses
    {
        get => EmptyInverses;
    }
}
