// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// A concrete TRAM object type that can be instantiated.
/// </summary>
public sealed class TramClass : TramObjectType
{
    /// <summary>
    /// Creates a new <see cref="TramClass"/> with the given singular and plural names.
    /// </summary>
    public TramClass(TramSchema schema, string singularName, string pluralName)
        : base(schema, singularName, pluralName)
    {
    }
}
