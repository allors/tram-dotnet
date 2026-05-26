// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram;

using System.Collections.Generic;
using Allors.Tram.Schema;

/// <summary>
/// The set of changes accumulated since the last checkpoint. Consumed by derivations to compute incremental updates.
/// </summary>
public interface IChangeSet
{
    /// <summary>
    /// True when any object, attribute, role or inverse changed since the last checkpoint.
    /// </summary>
    bool HasChanges { get; }

    /// <summary>
    /// Handles of objects created since the last checkpoint, in no defined order.
    /// </summary>
    IEnumerable<Handle> CreatedObjects { get; }

    /// <summary>
    /// Handles of objects deleted since the last checkpoint, in no defined order.
    /// </summary>
    IEnumerable<Handle> DeletedObjects { get; }

    /// <summary>
    /// Attributes whose value changed on at least one object since the last checkpoint.
    /// </summary>
    IEnumerable<TramAttribute> ChangedAttributes { get; }

    /// <summary>
    /// Roles whose value changed on at least one object since the last checkpoint.
    /// </summary>
    IEnumerable<ITramRole> ChangedRoles { get; }

    /// <summary>
    /// Inverses whose value changed on at least one object since the last checkpoint.
    /// </summary>
    IEnumerable<ITramInverse> ChangedInverses { get; }

    /// <summary>
    /// Handles of objects whose value of the given attribute changed since the last checkpoint.
    /// </summary>
    IEnumerable<Handle> ChangedObjects(TramAttribute attribute);

    /// <summary>
    /// Handles of objects whose value of the given role changed since the last checkpoint.
    /// </summary>
    IEnumerable<Handle> ChangedObjects(ITramRole role);

    /// <summary>
    /// Handles of objects whose value of the given inverse changed since the last checkpoint.
    /// </summary>
    IEnumerable<Handle> ChangedObjects(ITramInverse inverse);
}
