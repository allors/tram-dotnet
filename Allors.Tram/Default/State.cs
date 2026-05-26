// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Default;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Allors.Tram.Schema;

internal sealed class State(Deduplicator deduplicator)
{
    private uint handleCounter;

    // Primary
    private HashSet<Handle> primaryNewObjects = [];
    private HashSet<Handle> primaryDeletedObjects = [];

    private IImmutableDictionary<Handle, TramClass> primaryClassByObject = ImmutableDictionary<Handle, TramClass>.Empty;
    private Dictionary<TramAttribute, IDictionary<Handle, object?>> primaryValueRoleByObjectByAttribute = [];
    private Dictionary<ITramToOneRole, IDictionary<Handle, Handle>> primaryToOneRoleByObjectByRole = [];
    private Dictionary<ITramToManyRole, IDictionary<Handle, Handles>> primaryToManyRoleByObjectByRole = [];
    private Dictionary<ITramInverse, IDictionary<Handle, Handle>> primaryOneToInverseByRoleByInverse = [];
    private Dictionary<ITramInverse, IDictionary<Handle, Handles>> primaryManyToInverseByRoleByInverse = [];

    // Secondary
    private HashSet<Handle> secondaryNewObjects = [];
    private HashSet<Handle> secondaryDeletedObjects = [];

    private IImmutableDictionary<Handle, TramClass> secondaryClassByObject = ImmutableDictionary<Handle, TramClass>.Empty;
    private Dictionary<TramAttribute, IDictionary<Handle, object?>> secondaryValueRoleByObjectByAttribute = [];
    private Dictionary<ITramToOneRole, IDictionary<Handle, Handle>> secondaryToOneRoleByObjectByRole = [];
    private Dictionary<ITramToManyRole, IDictionary<Handle, Handles>> secondaryToManyRoleByObjectByRole = [];
    private Dictionary<ITramInverse, IDictionary<Handle, Handle>> secondaryOneToInverseByRoleByInverse = [];
    private Dictionary<ITramInverse, IDictionary<Handle, Handles>> secondaryManyToInverseByRoleByInverse = [];

    // Tertiary
    private readonly Dictionary<TramAttribute, ImmutableDictionary<Handle, object?>> tertiaryValueRoleByObjectByAttribute = [];
    private readonly Dictionary<ITramToOneRole, ImmutableDictionary<Handle, Handle>> tertiaryToOneRoleByObjectByRole = [];
    private readonly Dictionary<ITramToManyRole, ImmutableDictionary<Handle, Handles>> tertiaryToManyRoleByObjectByRole = [];
    private readonly Dictionary<ITramInverse, ImmutableDictionary<Handle, Handle>> tertiaryOneToInverseByRoleByInverse = [];
    private readonly Dictionary<ITramInverse, ImmutableDictionary<Handle, Handles>> tertiaryManyToInverseByRoleByInverse = [];
    private IImmutableDictionary<Handle, TramClass> tertiaryClassByObject = ImmutableDictionary<Handle, TramClass>.Empty;

    internal IEnumerable<Handle> Objects => this.primaryClassByObject.Keys;

    internal ChangeSet Checkpoint()
    {
        var newObjects = deduplicator.FromUnordered(this.primaryNewObjects);
        var deletedObjects = deduplicator.FromUnordered(this.primaryDeletedObjects);

        var added = deduplicator.Except(newObjects, deletedObjects);
        var removed = deduplicator.Except(deletedObjects, newObjects);

        var changeSet = new ChangeSet(added, removed);

        this.primaryNewObjects = [];
        this.primaryDeletedObjects = [];

        this.secondaryClassByObject = this.primaryClassByObject;

        foreach (var (attribute, primaryRoleByObject) in this.primaryValueRoleByObjectByAttribute)
        {
            if (!this.secondaryValueRoleByObjectByAttribute.TryGetValue(attribute, out var secondaryRoleByObject))
            {
                this.secondaryValueRoleByObjectByAttribute[attribute] = primaryRoleByObject;
                changeSet.ChangedObjectsByAttribute[attribute] = deduplicator.FromUnordered(primaryRoleByObject.Keys);
            }
            else
            {
                List<Handle>? changed = null;
                foreach (var (@object, role) in primaryRoleByObject)
                {
                    secondaryRoleByObject.TryGetValue(@object, out var mergedRole);

                    if (Equals(mergedRole, role))
                    {
                        continue;
                    }

                    secondaryRoleByObject[@object] = role;

                    changed ??= [];
                    changed.Add(@object);
                }

                if (changed != null)
                {
                    changeSet.ChangedObjectsByAttribute[attribute] = deduplicator.FromUnordered(changed);
                }
            }
        }

        foreach (var (role, primaryRoleByObject) in this.primaryToOneRoleByObjectByRole)
        {
            if (!this.secondaryToOneRoleByObjectByRole.TryGetValue(role, out var secondaryRoleByObject))
            {
                this.secondaryToOneRoleByObjectByRole[role] = primaryRoleByObject;
                changeSet.ChangedObjectsByRole[role] = deduplicator.FromUnordered(primaryRoleByObject.Keys);
            }
            else
            {
                List<Handle>? changed = null;
                foreach (var (@object, value) in primaryRoleByObject)
                {
                    secondaryRoleByObject.TryGetValue(@object, out var mergedRole);

                    if (Equals(mergedRole, value))
                    {
                        continue;
                    }

                    secondaryRoleByObject[@object] = value;

                    changed ??= [];
                    changed.Add(@object);
                }

                if (changed != null)
                {
                    changeSet.ChangedObjectsByRole[role] = deduplicator.FromUnordered(changed);
                }
            }
        }

        foreach (var (role, primaryRoleByObject) in this.primaryToManyRoleByObjectByRole)
        {
            if (!this.secondaryToManyRoleByObjectByRole.TryGetValue(role, out var secondaryRoleByObject))
            {
                this.secondaryToManyRoleByObjectByRole[role] = primaryRoleByObject;
                changeSet.ChangedObjectsByRole[role] = deduplicator.FromUnordered(primaryRoleByObject.Keys);
            }
            else
            {
                List<Handle>? changed = null;
                foreach (var (@object, value) in primaryRoleByObject)
                {
                    secondaryRoleByObject.TryGetValue(@object, out var mergedRole);

                    if (Same(mergedRole, value))
                    {
                        continue;
                    }

                    secondaryRoleByObject[@object] = value;

                    changed ??= [];
                    changed.Add(@object);
                }

                if (changed != null)
                {
                    changeSet.ChangedObjectsByRole[role] = deduplicator.FromUnordered(changed);
                }
            }
        }

        foreach (var (inverse, primaryInverseByRole) in this.primaryOneToInverseByRoleByInverse)
        {
            if (!this.secondaryOneToInverseByRoleByInverse.TryGetValue(inverse, out var secondaryOneToInverseByRole))
            {
                this.secondaryOneToInverseByRoleByInverse[inverse] = primaryInverseByRole;
                changeSet.ChangedObjectsByInverse[inverse] = deduplicator.FromUnordered(primaryInverseByRole.Keys);
            }
            else
            {
                List<Handle>? changed = null;
                foreach (var (role, value) in primaryInverseByRole)
                {
                    secondaryOneToInverseByRole.TryGetValue(role, out var mergedInverse);

                    if (Equals(mergedInverse, value))
                    {
                        continue;
                    }

                    secondaryOneToInverseByRole[role] = value;

                    changed ??= [];
                    changed.Add(role);
                }

                if (changed != null)
                {
                    changeSet.ChangedObjectsByInverse[inverse] = deduplicator.FromUnordered(changed);
                }
            }
        }

        foreach (var (inverse, primaryInverseByRole) in this.primaryManyToInverseByRoleByInverse)
        {
            if (!this.secondaryManyToInverseByRoleByInverse.TryGetValue(inverse, out var secondaryManyToInverseByRole))
            {
                this.secondaryManyToInverseByRoleByInverse[inverse] = primaryInverseByRole;
                changeSet.ChangedObjectsByInverse[inverse] = deduplicator.FromUnordered(primaryInverseByRole.Keys);
            }
            else
            {
                List<Handle>? changed = null;
                foreach (var (role, value) in primaryInverseByRole)
                {
                    secondaryManyToInverseByRole.TryGetValue(role, out var mergedInverse);

                    if (Same(mergedInverse, value))
                    {
                        continue;
                    }

                    secondaryManyToInverseByRole[role] = value;

                    changed ??= [];
                    changed.Add(role);
                }

                if (changed != null)
                {
                    changeSet.ChangedObjectsByInverse[inverse] = deduplicator.FromUnordered(changed);
                }
            }
        }

        this.primaryValueRoleByObjectByAttribute = [];
        this.primaryToOneRoleByObjectByRole = [];
        this.primaryToManyRoleByObjectByRole = [];
        this.primaryOneToInverseByRoleByInverse = [];
        this.primaryManyToInverseByRoleByInverse = [];

        this.secondaryNewObjects.UnionWith(changeSet.Created);
        this.secondaryDeletedObjects.UnionWith(changeSet.Deleted);

        return changeSet;
    }

    internal void Commit()
    {
        this.tertiaryClassByObject = this.secondaryClassByObject;

        foreach (var (attribute, secondaryValueRoleByObject) in this.secondaryValueRoleByObjectByAttribute)
        {
            if (this.tertiaryValueRoleByObjectByAttribute.TryGetValue(attribute, out var tertiaryValueRoleByObject))
            {
                tertiaryValueRoleByObject = tertiaryValueRoleByObject.SetItems(secondaryValueRoleByObject);
            }
            else
            {
                tertiaryValueRoleByObject = secondaryValueRoleByObject.ToImmutableDictionary();
            }

            this.tertiaryValueRoleByObjectByAttribute[attribute] = tertiaryValueRoleByObject;
        }

        foreach (var (role, secondaryToOneRoleByObject) in this.secondaryToOneRoleByObjectByRole)
        {
            if (this.tertiaryToOneRoleByObjectByRole.TryGetValue(role, out var tertiaryToOneRoleByObject))
            {
                tertiaryToOneRoleByObject = tertiaryToOneRoleByObject.SetItems(secondaryToOneRoleByObject);
            }
            else
            {
                tertiaryToOneRoleByObject = secondaryToOneRoleByObject.ToImmutableDictionary();
            }

            this.tertiaryToOneRoleByObjectByRole[role] = tertiaryToOneRoleByObject;
        }

        foreach (var (role, secondaryToManyRoleByObject) in this.secondaryToManyRoleByObjectByRole)
        {
            var mappedToHandles = secondaryToManyRoleByObject
                .Select(kvp => new KeyValuePair<Handle, Handles>(kvp.Key, deduplicator.Deduplicate(kvp.Value)));

            if (this.tertiaryToManyRoleByObjectByRole.TryGetValue(role, out var tertiaryToManyRoleByObject))
            {
                tertiaryToManyRoleByObject = tertiaryToManyRoleByObject.SetItems(mappedToHandles);
            }
            else
            {
                tertiaryToManyRoleByObject = mappedToHandles.ToImmutableDictionary();
            }

            this.tertiaryToManyRoleByObjectByRole[role] = tertiaryToManyRoleByObject;
        }

        foreach (var (inverse, secondaryOneToInverseByRole) in this.secondaryOneToInverseByRoleByInverse)
        {
            if (this.tertiaryOneToInverseByRoleByInverse.TryGetValue(inverse, out var tertiaryOneToInverseByRole))
            {
                tertiaryOneToInverseByRole = tertiaryOneToInverseByRole.SetItems(secondaryOneToInverseByRole);
            }
            else
            {
                tertiaryOneToInverseByRole = secondaryOneToInverseByRole.ToImmutableDictionary();
            }

            this.tertiaryOneToInverseByRoleByInverse[inverse] = tertiaryOneToInverseByRole;
        }

        foreach (var (inverse, secondaryManyToInverseByRole) in this.secondaryManyToInverseByRoleByInverse)
        {
            var mappedToHandles = secondaryManyToInverseByRole
                .Select(kvp => new KeyValuePair<Handle, Handles>(kvp.Key, deduplicator.Deduplicate(kvp.Value)));

            if (this.tertiaryManyToInverseByRoleByInverse.TryGetValue(inverse, out var tertiaryManyToInverseByRole))
            {
                tertiaryManyToInverseByRole = tertiaryManyToInverseByRole.SetItems(mappedToHandles);
            }
            else
            {
                tertiaryManyToInverseByRole = mappedToHandles.ToImmutableDictionary();
            }

            this.tertiaryManyToInverseByRoleByInverse[inverse] = tertiaryManyToInverseByRole;
        }

        this.secondaryNewObjects = [];
        this.secondaryDeletedObjects = [];

        this.secondaryValueRoleByObjectByAttribute = [];
        this.secondaryToOneRoleByObjectByRole = [];
        this.secondaryToManyRoleByObjectByRole = [];
        this.secondaryOneToInverseByRoleByInverse = [];
        this.secondaryManyToInverseByRoleByInverse = [];
    }

    internal void Rollback()
    {
        this.primaryNewObjects = [];
        this.primaryDeletedObjects = [];

        this.primaryClassByObject = this.tertiaryClassByObject;
        this.primaryValueRoleByObjectByAttribute = [];
        this.primaryToOneRoleByObjectByRole = [];
        this.primaryToManyRoleByObjectByRole = [];
        this.primaryOneToInverseByRoleByInverse = [];
        this.primaryManyToInverseByRoleByInverse = [];

        this.secondaryNewObjects = [];
        this.secondaryDeletedObjects = [];

        this.secondaryClassByObject = this.tertiaryClassByObject;
        this.secondaryValueRoleByObjectByAttribute = [];
        this.secondaryToOneRoleByObjectByRole = [];
        this.secondaryToManyRoleByObjectByRole = [];
        this.secondaryOneToInverseByRoleByInverse = [];
        this.secondaryManyToInverseByRoleByInverse = [];
    }

    internal TramClass GetClass(Handle @object)
    {
        return this.primaryClassByObject[@object];
    }

    internal Handle Create(TramClass @class)
    {
        var @new = new Handle(++this.handleCounter);
        this.primaryClassByObject = this.primaryClassByObject.Add(@new, @class);
        this.primaryNewObjects.Add(@new);
        return @new;
    }

    internal void Delete(Handle @object)
    {
        this.primaryClassByObject = this.primaryClassByObject.Remove(@object);
        this.primaryDeletedObjects.Add(@object);
    }

    internal bool Exists(Handle @object)
    {
        return this.primaryClassByObject.ContainsKey(@object);
    }

    internal object? GetValueRole(Handle @object, TramAttribute attribute)
    {
        if (this.primaryValueRoleByObjectByAttribute.TryGetValue(attribute, out var primaryRoleByObject) &&
            primaryRoleByObject.TryGetValue(@object, out var primaryRole))
        {
            return primaryRole;
        }

        if (this.secondaryValueRoleByObjectByAttribute.TryGetValue(attribute, out var secondaryRoleByObject) &&
            secondaryRoleByObject.TryGetValue(@object, out var secondaryRole))
        {
            return secondaryRole;
        }

        if (this.tertiaryValueRoleByObjectByAttribute.TryGetValue(attribute, out var tertiaryRoleByObject) &&
            tertiaryRoleByObject.TryGetValue(@object, out var tertiaryRole))
        {
            return tertiaryRole;
        }

        return null;
    }

    internal void SetValueRole(Handle @object, TramAttribute attribute, object? normalizedRole)
    {
        if (!this.primaryValueRoleByObjectByAttribute.TryGetValue(attribute, out var primaryValueRoleByObject))
        {
            primaryValueRoleByObject = new Dictionary<Handle, object?>();
            this.primaryValueRoleByObjectByAttribute[attribute] = primaryValueRoleByObject;
        }

        primaryValueRoleByObject[@object] = normalizedRole;
    }

    internal Handle GetToOneRole(Handle @object, ITramToOneRole role)
    {
        if (this.primaryToOneRoleByObjectByRole.TryGetValue(role, out var primaryRoleByObject) &&
            primaryRoleByObject.TryGetValue(@object, out var primaryRole))
        {
            return primaryRole;
        }

        if (this.secondaryToOneRoleByObjectByRole.TryGetValue(role, out var secondaryRoleByObject) &&
            secondaryRoleByObject.TryGetValue(@object, out var secondaryRole))
        {
            return secondaryRole;
        }

        if (this.tertiaryToOneRoleByObjectByRole.TryGetValue(role, out var tertiaryRoleByObject) &&
            tertiaryRoleByObject.TryGetValue(@object, out var tertiaryRole))
        {
            return tertiaryRole;
        }

        return default;
    }

    internal void SetToOneRole(Handle @object, ITramToOneRole role, Handle normalizedRole)
    {
        if (!this.primaryToOneRoleByObjectByRole.TryGetValue(role, out var primaryToOneRoleByObject))
        {
            primaryToOneRoleByObject = new Dictionary<Handle, Handle>();
            this.primaryToOneRoleByObjectByRole[role] = primaryToOneRoleByObject;
        }

        primaryToOneRoleByObject[@object] = normalizedRole;
    }

    internal Handles GetToManyRole(Handle @object, ITramToManyRole role)
    {
        if (this.primaryToManyRoleByObjectByRole.TryGetValue(role, out var primaryRoleByObject) &&
            primaryRoleByObject.TryGetValue(@object, out var primaryRole))
        {
            return primaryRole;
        }

        if (this.secondaryToManyRoleByObjectByRole.TryGetValue(role, out var secondaryRoleByObject) &&
            secondaryRoleByObject.TryGetValue(@object, out var secondaryRole))
        {
            return secondaryRole;
        }

        if (this.tertiaryToManyRoleByObjectByRole.TryGetValue(role, out var tertiaryRoleByObject) &&
            tertiaryRoleByObject.TryGetValue(@object, out var tertiaryRole))
        {
            return tertiaryRole;
        }

        return Handles.Empty;
    }

    internal void SetToManyRole(Handle @object, ITramToManyRole role, Handles normalizedRole)
    {
        if (!this.primaryToManyRoleByObjectByRole.TryGetValue(role, out var primaryToManyRoleByObject))
        {
            primaryToManyRoleByObject = new Dictionary<Handle, Handles>();
            this.primaryToManyRoleByObjectByRole[role] = primaryToManyRoleByObject;
        }

        primaryToManyRoleByObject[@object] = normalizedRole;
    }

    internal Handle GetOneToInverse(Handle role, ITramToOneInverse inverse)
    {
        if (this.primaryOneToInverseByRoleByInverse.TryGetValue(inverse, out var primaryOneToInverseByRole) &&
            primaryOneToInverseByRole.TryGetValue(role, out var primaryOneToInverse))
        {
            return primaryOneToInverse;
        }

        if (this.secondaryOneToInverseByRoleByInverse.TryGetValue(inverse, out var secondaryOneToInverseByRole) &&
            secondaryOneToInverseByRole.TryGetValue(role, out var secondaryOneToInverse))
        {
            return secondaryOneToInverse;
        }

        if (this.tertiaryOneToInverseByRoleByInverse.TryGetValue(inverse, out var tertiaryOneToInverseByRole) &&
            tertiaryOneToInverseByRole.TryGetValue(role, out var tertiaryOneToInverse))
        {
            return tertiaryOneToInverse;
        }

        return default;
    }

    internal void SetOneToInverse(Handle role, ITramToOneInverse inverse, Handle value)
    {
        if (!this.primaryOneToInverseByRoleByInverse.TryGetValue(inverse, out var primaryOneToInverseByRole))
        {
            primaryOneToInverseByRole = new Dictionary<Handle, Handle>();
            this.primaryOneToInverseByRoleByInverse[inverse] = primaryOneToInverseByRole;
        }

        primaryOneToInverseByRole[role] = value;
    }

    internal void RemoveOneToInverse(Handle role, ITramToOneInverse inverse)
    {
        if (!this.primaryOneToInverseByRoleByInverse.TryGetValue(inverse, out var primaryOneToInverseByRole))
        {
            primaryOneToInverseByRole = new Dictionary<Handle, Handle>();
            this.primaryOneToInverseByRoleByInverse[inverse] = primaryOneToInverseByRole;
        }

        primaryOneToInverseByRole[role] = default;
    }

    internal Handles GetManyToInverse(Handle role, ITramToManyInverse inverse)
    {
        if (this.primaryManyToInverseByRoleByInverse.TryGetValue(inverse, out var primaryManyToInverseByRole) &&
            primaryManyToInverseByRole.TryGetValue(role, out var primaryManyToInverse))
        {
            return primaryManyToInverse;
        }

        if (this.secondaryManyToInverseByRoleByInverse.TryGetValue(inverse, out var secondaryManyToInverseByRole) &&
            secondaryManyToInverseByRole.TryGetValue(role, out var secondaryManyToInverse))
        {
            return secondaryManyToInverse;
        }

        if (this.tertiaryManyToInverseByRoleByInverse.TryGetValue(inverse, out var tertiaryManyToInverseByRole) &&
            tertiaryManyToInverseByRole.TryGetValue(role, out var tertiaryManyToInverse))
        {
            return tertiaryManyToInverse;
        }

        return Handles.Empty;
    }

    internal void SetManyToInverse(Handle role, ITramToManyInverse inverse, Handles value)
    {
        if (!this.primaryManyToInverseByRoleByInverse.TryGetValue(inverse, out var primaryManyToInverseByRole))
        {
            primaryManyToInverseByRole = new Dictionary<Handle, Handles>();
            this.primaryManyToInverseByRoleByInverse[inverse] = primaryManyToInverseByRole;
        }

        primaryManyToInverseByRole[role] = value;
    }

    private static bool Same(Handles? source, Handles? destination)
    {
        if (source != null)
        {
            return source.Equals(destination);
        }

        return destination?.Equals(source) != false;
    }
}
