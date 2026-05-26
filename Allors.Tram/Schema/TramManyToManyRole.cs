// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// The writable, multi-valued end of a many-to-many bidirectional relationship.
/// </summary>
public sealed class TramManyToManyRole : TramRole<TramManyToManyInverse>, ITramToManyRole
{
    internal TramManyToManyRole(TramObjectType declaringType, TramObjectType type, string singularName, string pluralName)
        : base(declaringType, type, singularName, pluralName)
    {
    }

    /// <inheritdoc/>
    public override bool IsOne => false;

    /// <summary>
    /// Deconstructs this role into its matching inverse and itself.
    /// </summary>
    public void Deconstruct(out TramManyToManyInverse inverse, out TramManyToManyRole role)
    {
        inverse = this.Inverse;
        role = this;
    }
}
