// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// The read-only, multi-valued reverse end of a many-to-many bidirectional relationship.
/// </summary>
public sealed class TramManyToManyInverse : TramInverse<TramManyToManyRole>, ITramToManyInverse
{
    internal TramManyToManyInverse(TramObjectType declaringType, TramObjectType type, TramManyToManyRole role, string singularName, string pluralName)
        : base(declaringType, type, role, singularName, pluralName)
    {
    }

    /// <inheritdoc/>
    public override bool IsOne => false;
}
