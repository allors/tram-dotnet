// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// The read-only reverse end of a one-to-one bidirectional relationship.
/// </summary>
public sealed class TramOneToOneInverse : TramInverse<TramOneToOneRole>, ITramToOneInverse
{
    internal TramOneToOneInverse(TramObjectType declaringType, TramObjectType type, TramOneToOneRole role, string singularName, string pluralName)
        : base(declaringType, type, role, singularName, pluralName)
    {
    }

    /// <inheritdoc/>
    public override bool IsOne => true;
}
