// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// The read-only, single-valued reverse end of a one-to-many bidirectional relationship.
/// </summary>
public sealed class TramOneToManyInverse : TramInverse<TramOneToManyRole>, ITramToOneInverse
{
    internal TramOneToManyInverse(TramObjectType declaringType, TramObjectType type, TramOneToManyRole role, string singularName, string pluralName)
        : base(declaringType, type, role, singularName, pluralName)
    {
    }

    /// <inheritdoc/>
    public override bool IsOne => true;
}
