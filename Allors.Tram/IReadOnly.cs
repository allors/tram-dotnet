// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram;

using System.Collections.Generic;
using Allors.Tram.Schema;

/// <summary>
/// Read-only access to the objects, attributes and relationships managed by a TRAM.
/// </summary>
public interface IReadOnly
{
    /// <summary>
    /// The schema that defines the object types, attributes and relationships of this TRAM.
    /// </summary>
    TramSchema Schema { get; }

    /// <summary>
    /// Returns the class of the given object.
    /// </summary>
    TramClass GetClass(Handle @object);

    /// <summary>
    /// Returns the handles of every object in this TRAM, in no defined order.
    /// </summary>
    IEnumerable<Handle> Objects();

    /// <summary>
    /// Returns the handles of every object whose class is assignable to <paramref name="objectType"/>, in no defined order.
    /// </summary>
    IEnumerable<Handle> ObjectsOfType(TramObjectType objectType);

    /// <summary>
    /// Returns true when an object with the given handle exists in this TRAM.
    /// </summary>
    bool Exists(Handle @object);

    /// <summary>
    /// Returns the value of the given attribute on the given object, or null when unset.
    /// </summary>
    object? Get(Handle @object, TramAttribute attribute);

    /// <summary>
    /// Returns the handle related to the given object through the given to-one role or inverse, or the null handle when unset.
    /// </summary>
    Handle Get(Handle @object, ITramToOneRelationEnd role);

    /// <summary>
    /// Returns the handles related to the given object through the given to-many role or inverse, in no defined order.
    /// </summary>
    IEnumerable<Handle> Get(Handle @object, ITramToManyRelationEnd role);

    /// <summary>
    /// Tries to get the value of the given attribute on the given object. Returns false when the value is unset.
    /// </summary>
    bool TryGet(Handle @object, TramAttribute attribute, out object? value);

    /// <summary>
    /// Tries to get the handle related through the given to-one role or inverse. Returns false when unset.
    /// </summary>
    bool TryGet(Handle @object, ITramToOneRelationEnd role, out Handle item);

    /// <summary>
    /// Tries to get the handles related through the given to-many role or inverse. Returns false when the set is empty.
    /// </summary>
    bool TryGet(Handle @object, ITramToManyRelationEnd role, out IEnumerable<Handle> items);
}
