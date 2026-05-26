// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// Base class for the read-only reverse end of a bidirectional relationship.
/// </summary>
/// <typeparam name="TRole">The concrete type of the matching role.</typeparam>
public abstract class TramInverse<TRole> : ITramInverse
    where TRole : ITramRole
{
    /// <summary>
    /// Creates a new inverse declared on <paramref name="declaringType"/> and mirroring <paramref name="role"/>.
    /// </summary>
    protected TramInverse(TramObjectType declaringType, TramObjectType type, TRole role, string singularName, string pluralName)
    {
        this.DeclaringType = declaringType;
        this.Type = type;
        this.Role = role;
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
    /// The role this inverse mirrors.
    /// </summary>
    public TRole Role { get; }

    /// <inheritdoc/>
    public ITramRelationEnd OtherEnd => this.Role;

    ITramRole ITramInverse.Role => this.Role;

    /// <inheritdoc/>
    public string SingularName { get; }

    /// <inheritdoc/>
    public string PluralName { get; }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public override string ToString() => this.Name;
}
