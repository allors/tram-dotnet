// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// The read-only, multi-valued reverse end of a many-to-one bidirectional relationship.
/// </summary>
public sealed class TramManyToOneInverse : TramInverse<TramManyToOneRole>, ITramToManyInverse
{
    internal TramManyToOneInverse(TramObjectType declaringType, TramObjectType type, TramManyToOneRole role, string singularName, string pluralName)
        : base(declaringType, type, role, singularName, pluralName)
    {
    }

    /// <inheritdoc/>
    public override bool IsOne => false;
}
