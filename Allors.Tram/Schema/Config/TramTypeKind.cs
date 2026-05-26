// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema.Config;

/// <summary>
/// The kind of a TRAM type declared in schema configuration.
/// </summary>
public enum TramTypeKind
{
    /// <summary>
    /// A value type (the value of an attribute).
    /// </summary>
    ValueType = 0,

    /// <summary>
    /// An object interface type.
    /// </summary>
    Interface = 1,

    /// <summary>
    /// A concrete object class type.
    /// </summary>
    Class = 2,
}
