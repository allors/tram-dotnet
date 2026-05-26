// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// Common shape of any property (attribute or relation end) declared on an object type.
/// </summary>
public interface ITramProperty
{
    /// <summary>
    /// The object type on which this property is declared.
    /// </summary>
    TramObjectType DeclaringType { get; }

    /// <summary>
    /// The singular form of the property's name.
    /// </summary>
    string SingularName { get; }

    /// <summary>
    /// The plural form of the property's name.
    /// </summary>
    string PluralName { get; }

    /// <summary>
    /// The default name of the property (singular for to-one, plural for to-many).
    /// </summary>
    string Name { get; }
}
