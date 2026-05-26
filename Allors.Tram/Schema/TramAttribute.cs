// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// A value-typed property declared on a specific object type.
/// </summary>
public sealed class TramAttribute : ITramAttribute
{
    internal TramAttribute(TramObjectType declaringType, TramValueType type, string singularName, string pluralName)
    {
        this.DeclaringType = declaringType;
        this.Type = type;
        this.SingularName = singularName;
        this.PluralName = pluralName;
        this.Name = singularName;
    }

    /// <inheritdoc/>
    public TramObjectType DeclaringType { get; }

    /// <inheritdoc/>
    public TramValueType Type { get; }

    /// <inheritdoc/>
    public string SingularName { get; }

    /// <inheritdoc/>
    public string PluralName { get; }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public string FullName { get; internal set; } = null!;

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.Name;
    }
}
