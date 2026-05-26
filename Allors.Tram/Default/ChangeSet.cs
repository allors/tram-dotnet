// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Default;

using System.Collections.Generic;
using Allors.Tram.Schema;

internal sealed class ChangeSet(
    Handles created,
    Handles deleted)
    : IChangeSet
{
    private bool? hasChanges;

    IEnumerable<Handle> IChangeSet.CreatedObjects => this.Created;

    IEnumerable<Handle> IChangeSet.DeletedObjects => this.Deleted;

    IEnumerable<TramAttribute> IChangeSet.ChangedAttributes
    {
        get => this.ChangedObjectsByAttribute.Keys;
    }

    IEnumerable<ITramRole> IChangeSet.ChangedRoles
    {
        get => this.ChangedObjectsByRole.Keys;
    }

    IEnumerable<ITramInverse> IChangeSet.ChangedInverses
    {
        get => this.ChangedObjectsByInverse.Keys;
    }

    bool IChangeSet.HasChanges => this.hasChanges ??=
        !this.Created.IsEmpty ||
        !this.Deleted.IsEmpty ||
        this.ChangedObjectsByAttribute.Count != 0 ||
        this.ChangedObjectsByRole.Count != 0 ||
        this.ChangedObjectsByInverse.Count != 0;

    internal Handles Created { get; } = created;

    internal Handles Deleted { get; } = deleted;

    internal Dictionary<TramAttribute, Handles> ChangedObjectsByAttribute { get; } = [];

    internal Dictionary<ITramRole, Handles> ChangedObjectsByRole { get; } = [];

    internal Dictionary<ITramInverse, Handles> ChangedObjectsByInverse { get; } = [];

    IEnumerable<Handle> IChangeSet.ChangedObjects(TramAttribute attribute)
    {
        this.ChangedObjectsByAttribute.TryGetValue(attribute, out var changedObjects);
        return changedObjects ?? Handles.Empty;
    }

    IEnumerable<Handle> IChangeSet.ChangedObjects(ITramRole role)
    {
        this.ChangedObjectsByRole.TryGetValue(role, out var changedObjects);
        return changedObjects ?? Handles.Empty;
    }

    IEnumerable<Handle> IChangeSet.ChangedObjects(ITramInverse inverse)
    {
        this.ChangedObjectsByInverse.TryGetValue(inverse, out var changedObjects);
        return changedObjects ?? Handles.Empty;
    }
}
