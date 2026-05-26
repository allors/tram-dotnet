// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// One end of a bidirectional relationship: either a role (writable) or its inverse (read-only).
/// </summary>
public interface ITramRelationEnd : ITramTypedProperty<TramObjectType>
{
    /// <summary>
    /// The opposite end of the relationship.
    /// </summary>
    ITramRelationEnd OtherEnd { get; }

    /// <summary>
    /// True when this end is single-valued (to-one).
    /// </summary>
    bool IsOne { get; }

    /// <summary>
    /// True when this end is multi-valued (to-many).
    /// </summary>
    bool IsMany { get; }
}
