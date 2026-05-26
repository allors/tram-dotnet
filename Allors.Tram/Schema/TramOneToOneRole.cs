// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// The writable end of a one-to-one bidirectional relationship.
/// </summary>
public sealed class TramOneToOneRole : TramRole<TramOneToOneInverse>, ITramToOneRole
{
    internal TramOneToOneRole(TramObjectType declaringType, TramObjectType type, string singularName, string pluralName)
        : base(declaringType, type, singularName, pluralName)
    {
    }

    /// <inheritdoc/>
    public override bool IsOne => true;

    /// <summary>
    /// Deconstructs this role into its matching inverse and itself.
    /// </summary>
    public void Deconstruct(out TramOneToOneInverse inverse, out TramOneToOneRole role)
    {
        inverse = this.Inverse;
        role = this;
    }
}
