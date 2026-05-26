// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// The writable, single-valued end of a many-to-one bidirectional relationship.
/// </summary>
public sealed class TramManyToOneRole : TramRole<TramManyToOneInverse>, ITramToOneRole
{
    internal TramManyToOneRole(TramObjectType declaringType, TramObjectType type, string singularName, string pluralName)
        : base(declaringType, type, singularName, pluralName)
    {
    }

    /// <inheritdoc/>
    public override bool IsOne => true;

    /// <summary>
    /// Deconstructs this role into its matching inverse and itself.
    /// </summary>
    public void Deconstruct(out TramManyToOneInverse inverse, out TramManyToOneRole role)
    {
        inverse = this.Inverse;
        role = this;
    }
}
