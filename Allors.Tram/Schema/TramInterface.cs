// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// An abstract TRAM object type that other object types can inherit from. Cannot itself be instantiated.
/// </summary>
public sealed class TramInterface : TramObjectType
{
    /// <summary>
    /// Creates a new <see cref="TramInterface"/> with the given singular and plural names.
    /// </summary>
    public TramInterface(TramSchema schema, string singularName, string pluralName)
        : base(schema, singularName, pluralName)
    {
    }
}
