// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// Base class for the writable forward end of a bidirectional relationship.
/// </summary>
/// <typeparam name="TInverse">The concrete type of the matching inverse end.</typeparam>
public abstract class TramRole<TInverse> : ITramRole
    where TInverse : ITramInverse
{
    /// <summary>
    /// Creates a new role declared on <paramref name="declaringType"/> and pointing at <paramref name="type"/>.
    /// </summary>
    protected TramRole(TramObjectType declaringType, TramObjectType type, string singularName, string pluralName)
    {
        this.DeclaringType = declaringType;
        this.Type = type;
        this.SingularName = singularName;
        this.PluralName = pluralName;
        this.Name = this.IsOne ? singularName : pluralName;
    }

    /// <inheritdoc/>
    public abstract bool IsOne { get; }

    /// <inheritdoc/>
    public bool IsMany => !this.IsOne;

    /// <inheritdoc/>
    public TramObjectType DeclaringType { get; }

    /// <inheritdoc/>
    public TramObjectType Type { get; }

    /// <summary>
    /// The matching inverse end of this role.
    /// </summary>
    public TInverse Inverse { get; internal set; } = default!;

    /// <inheritdoc/>
    public ITramRelationEnd OtherEnd => this.Inverse;

    ITramInverse ITramRole.Inverse => this.Inverse;

    /// <inheritdoc/>
    public string SingularName { get; }

    /// <inheritdoc/>
    public string PluralName { get; }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public string FullName { get; internal set; } = null!;

    /// <inheritdoc/>
    public override string ToString() => this.Name;

    internal string SingularNameForInverse(TramType tramType) => $"{tramType.Name}Where{this.SingularName}";

    internal string PluralNameForInverse(TramType tramType) => $"{TramSchema.Pluralize(tramType.Name)}Where{this.SingularName}";
}
