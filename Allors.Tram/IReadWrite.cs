// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram;

using System.Collections.Generic;
using Allors.Tram.Schema;

/// <summary>
/// Read and write access to the objects, attributes and relationships managed by a TRAM.
/// </summary>
public interface IReadWrite : IReadOnly
{
    /// <summary>
    /// Creates a new object of the given class and returns its handle.
    /// </summary>
    Handle Create(TramClass @class);

    /// <summary>
    /// Deletes the given object from this TRAM. Unknown or already-deleted handles are ignored.
    /// </summary>
    void Delete(Handle @object);

    /// <summary>
    /// Sets the value of the given attribute on the given object. A null value removes the attribute.
    /// </summary>
    void Set(Handle @object, TramAttribute attribute, object? value);

    /// <summary>
    /// Sets the to-one role on the given object. The null handle removes the relation.
    /// </summary>
    void Set(Handle @object, ITramToOneRole role, Handle item);

    /// <summary>
    /// Replaces the entire to-many role on the given object with the given handles.
    /// </summary>
    void Set(Handle @object, ITramToManyRole role, IEnumerable<Handle> items);

    /// <summary>
    /// Adds the given handle to the to-many role on the given object.
    /// </summary>
    void Add(Handle @object, ITramToManyRole role, Handle item);

    /// <summary>
    /// Adds the given handles to the to-many role on the given object.
    /// </summary>
    void Add(Handle @object, ITramToManyRole role, IEnumerable<Handle> items);

    /// <summary>
    /// Removes the given handle from the to-many role on the given object.
    /// </summary>
    void Remove(Handle @object, ITramToManyRole role, Handle item);

    /// <summary>
    /// Removes the given handles from the to-many role on the given object.
    /// </summary>
    void Remove(Handle @object, ITramToManyRole role, IEnumerable<Handle> items);
}
