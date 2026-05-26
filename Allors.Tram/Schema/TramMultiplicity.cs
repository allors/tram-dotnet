// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// The cardinality of a bidirectional relationship.
/// </summary>
public enum TramMultiplicity
{
    /// <summary>
    /// One object on each end.
    /// </summary>
    OneToOne,

    /// <summary>
    /// One object on the role end, many on the inverse end.
    /// </summary>
    OneToMany,

    /// <summary>
    /// Many objects on the role end, one on the inverse end.
    /// </summary>
    ManyToOne,

    /// <summary>
    /// Many objects on each end.
    /// </summary>
    ManyToMany,
}
