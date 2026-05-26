// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// A property whose value is constrained to a specific TRAM type.
/// </summary>
/// <typeparam name="TType">The kind of TRAM type that constrains the property's value.</typeparam>
public interface ITramTypedProperty<out TType> : ITramProperty
    where TType : TramType
{
    /// <summary>
    /// The TRAM type that constrains this property's value.
    /// </summary>
    TType Type { get; }
}
