// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// A value-typed property declared on an object type.
/// </summary>
public interface ITramAttribute : ITramTypedProperty<TramValueType>
{
    /// <summary>
    /// The fully-qualified name of this attribute, including its declaring type.
    /// </summary>
    string FullName { get; }
}
