// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Default;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Tram.Schema;

/// <summary>
/// In-memory implementation of <see cref="ITram"/> with full read/write and derivation support.
/// </summary>
public sealed class Tram : ITram
{
    private readonly Func<TramAttribute, object?, object?>? normalize;
    private readonly Deduplicator deduplicator;
    private readonly State state;
    private readonly Derivations derivations;

    /// <summary>
    /// Creates a new in-memory TRAM bound to the given schema, optional derivations, cycle limit and attribute-value normaliser.
    /// </summary>
    public Tram(
        TramSchema schema,
        IEnumerable<IDerivation>? derivations = null,
        int maxCycles = Derivations.DefaultMaxCycles,
        Func<TramAttribute, object?, object?>? normalize = null)
    {
        this.Schema = schema;
        this.normalize = normalize;
        this.deduplicator = new Deduplicator();
        this.state = new State(this.deduplicator);
        this.derivations = new Derivations(this, derivations ?? [], maxCycles);
    }

    /// <inheritdoc/>
    public TramSchema Schema { get; }

    /// <inheritdoc/>
    public IEnumerable<Handle> Objects() => this.state.Objects;

    /// <inheritdoc/>
    public IEnumerable<Handle> ObjectsOfType(TramObjectType objectType) => this.Objects().Where(v => objectType.IsAssignableFrom(this.GetClass(v)));

    /// <inheritdoc/>
    public TramClass GetClass(Handle @object)
    {
        this.Assert(@object);
        return this.state.GetClass(@object);
    }

    /// <inheritdoc/>
    public void Derive()
    {
        try
        {
            this.derivations.Derive();
            this.state.Commit();
        }
        catch
        {
            this.state.Rollback();
            throw;
        }
    }

    /// <inheritdoc/>
    public Handle Create(TramClass @class)
    {
        return this.state.Create(@class);
    }

    /// <inheritdoc/>
    public bool Exists(Handle @object) => this.state.Exists(@object);

    /// <inheritdoc/>
    public void Delete(Handle @object)
    {
        // Check if object is already deleted
        if (!this.Exists(@object))
        {
            throw new ArgumentException("Object is already deleted.");
        }

        var objectType = this.GetClass(@object);

        // Attributes
        foreach (var attribute in objectType.Attributes.Cast<TramAttribute>())
        {
            this.Set(@object, attribute, null);
        }

        // RelationEnds
        foreach (var role in objectType.Roles)
        {
            switch (role)
            {
            case ITramToOneRole toOneRole:
                this.Set(@object, toOneRole, default);
                break;
            default:
                var toManyRole = (ITramToManyRole)role;
                this.Set(@object, toManyRole, []);
                break;
            }
        }

        // Inverses
        foreach (var inverse in objectType.Inverses)
        {
            var role = inverse.Role;

            if (inverse is ITramToManyInverse manyToInverse)
            {
                var others = this.Get(@object, manyToInverse);

                foreach (var other in others)
                {
                    if (role is ITramToOneRole toOneRole)
                    {
                        this.Set(other, toOneRole, default);
                    }
                    else
                    {
                        var toManyRole = (ITramToManyRole)role;
                        this.Remove(other, toManyRole, @object);
                    }
                }
            }
            else
            {
                var oneToInverse = (ITramToOneInverse)inverse;
                this.TryGet(@object, oneToInverse, out var other);

                if (other.IsNull)
                {
                    continue;
                }

                if (role is ITramToOneRole toOneRole)
                {
                    this.Set(other, toOneRole, default);
                }
                else
                {
                    var toManyRole = (ITramToManyRole)role;
                    this.Remove(other, toManyRole, @object);
                }
            }
        }

        this.state.Delete(@object);
    }

    /// <inheritdoc/>
    public bool TryGet(Handle @object, TramAttribute attribute, out object? value)
    {
        value = this.Get(@object, attribute);
        return value != null;
    }

    /// <inheritdoc/>
    public bool TryGet(Handle @object, ITramToOneRelationEnd role, out Handle item)
    {
        item = this.Get(@object, role);
        return !item.IsNull;
    }

    bool IReadOnly.TryGet(Handle @object, ITramToManyRelationEnd role, out IEnumerable<Handle> items)
    {
        items = this.Get(@object, role);
        return items.Any();
    }

    /// <inheritdoc/>
    public object? Get(Handle @object, TramAttribute attribute)
    {
        this.Assert(@object);
        return this.state.GetValueRole(@object, attribute);
    }

    /// <inheritdoc/>
    public Handle Get(Handle @object, ITramToOneRelationEnd role)
    {
        this.Assert(@object);
        return role switch
        {
            ITramToOneRole toOneRole => this.state.GetToOneRole(@object, toOneRole),
            ITramToOneInverse toOneInverse => this.state.GetOneToInverse(@object, toOneInverse),
            _ => default,
        };
    }

    /// <inheritdoc/>
    public IEnumerable<Handle> Get(Handle @object, ITramToManyRelationEnd role)
    {
        this.Assert(@object);
        return role switch
        {
            ITramToManyRole toManyRole => this.state.GetToManyRole(@object, toManyRole),
            ITramToManyInverse toManyInverse => this.state.GetManyToInverse(@object, toManyInverse),
            _ => Handles.Empty,
        };
    }

    /// <inheritdoc/>
    public void Set(Handle @object, TramAttribute attribute, object? value)
    {
        this.Assert(@object);
        var normalized = this.Normalize(attribute, value);

        this.TryGet(@object, attribute, out var currentRole);
        if (Equals(currentRole, normalized))
        {
            return;
        }

        // Role
        this.state.SetValueRole(@object, attribute, normalized);
    }

    /// <inheritdoc/>
    public void Set(Handle @object, ITramToOneRole role, Handle item)
    {
        this.Assert(@object);
        var normalized = this.Normalize(role, item);

        switch (role)
        {
        case TramOneToOneRole oneToOneRole:
            if (normalized.IsNull)
            {
                this.RemoveOneToOneRole(@object, oneToOneRole);
                return;
            }

            this.SetOneToOneRole(@object, oneToOneRole, normalized);
            return;

        case TramManyToOneRole manyToOneRole:
            if (normalized.IsNull)
            {
                this.RemoveManyToOneRole(@object, manyToOneRole);
                return;
            }

            this.SetManyToOneRole(@object, manyToOneRole, normalized);
            return;

        default:
            throw new InvalidOperationException();
        }
    }

    /// <inheritdoc/>
    public void Set(Handle @object, ITramToManyRole role, IEnumerable<Handle> items)
    {
        this.Assert(@object);
        var normalized = this.Normalize(role, items);

        switch (role)
        {
        case TramOneToManyRole toManyRole:
            if (normalized.IsEmpty)
            {
                this.RemoveOneToManyRole(@object, toManyRole);
                return;
            }

            this.SetOneToManyRole(@object, toManyRole, normalized);
            return;

        case TramManyToManyRole toManyRole:
            if (normalized.IsEmpty)
            {
                this.RemoveManyToManyRole(@object, toManyRole);
                return;
            }

            this.SetManyToManyRole(@object, toManyRole, normalized);
            return;

        default:
            throw new InvalidOperationException();
        }
    }

    /// <inheritdoc/>
    public void Add(Handle @object, ITramToManyRole role, Handle item)
    {
        this.Assert(@object);
        var normalized = this.Normalize(role, item);

        if (normalized.IsNull)
        {
            return;
        }

        switch (role)
        {
        case TramOneToManyRole toManyRole:
            this.AddToOneToManyRole(@object, toManyRole, normalized);
            return;

        case TramManyToManyRole toManyRole:
            this.AddToManyToManyRole(@object, toManyRole, normalized);
            return;

        default:
            throw new InvalidOperationException();
        }
    }

    /// <inheritdoc/>
    public void Add(Handle @object, ITramToManyRole role, IEnumerable<Handle> items)
    {
        this.Assert(@object);
        var normalized = this.Normalize(role, items);

        if (normalized.Count == 0)
        {
            return;
        }

        switch (role)
        {
        case TramOneToManyRole toManyRole:
            this.AddToOneToManyRole(@object, toManyRole, normalized);
            return;

        case TramManyToManyRole toManyRole:
            this.AddToManyToManyRole(@object, toManyRole, normalized);
            return;

        default:
            throw new InvalidOperationException();
        }
    }

    /// <inheritdoc/>
    public void Remove(Handle @object, ITramToManyRole role, Handle item)
    {
        this.Assert(@object);
        var normalized = this.Normalize(role, item);

        if (normalized.IsNull)
        {
            return;
        }

        switch (role)
        {
        case TramOneToManyRole toManyRole:
            this.RemoveFromOneToManyRole(@object, toManyRole, normalized);
            return;

        case TramManyToManyRole toManyRole:
            this.RemoveFromManyToManyRole(@object, toManyRole, normalized);
            return;

        default:
            throw new InvalidOperationException();
        }
    }

    /// <inheritdoc/>
    public void Remove(Handle @object, ITramToManyRole role, IEnumerable<Handle> items)
    {
        this.Assert(@object);
        var normalized = this.Normalize(role, items);

        if (normalized.Count == 0)
        {
            return;
        }

        switch (role)
        {
        case TramOneToManyRole toManyRole:
            this.RemoveFromOneToManyRole(@object, toManyRole, normalized);
            return;

        case TramManyToManyRole toManyRole:
            this.RemoveObjectsFromManyToManyRole(@object, toManyRole, normalized);
            return;

        default:
            throw new InvalidOperationException();
        }
    }

    internal IChangeSet Checkpoint() => this.state.Checkpoint();

    private void SetOneToOneRole(Handle @object, TramOneToOneRole role, Handle normalized)
    {
        /*  [if exist]        [then remove]        set
         *
         *  RA ----- R         RA --x-- R       RA    -- R       RA    -- R
         *                ->                +        -        =       -
         *   A ----- PR         A --x-- PR       A --    PR       A --    PR
         */

        var previousRole = this.state.GetToOneRole(@object, role);

        // R = PR
        if (normalized.Equals(previousRole))
        {
            return;
        }

        var inverse = role.Inverse;

        // A --x-- PR
        if (!previousRole.IsNull)
        {
            this.RemoveOneToOneRole(@object, role);
        }

        this.TryGet(normalized, inverse, out var roleObject);

        // RA --x-- R
        if (!roleObject.IsNull)
        {
            this.RemoveOneToOneRole(roleObject, role);
        }

        // A <---- R
        this.state.SetOneToInverse(normalized, inverse, @object);

        // A ----> R
        this.state.SetToOneRole(@object, role, normalized);
    }

    private void RemoveOneToOneRole(Handle @object, TramOneToOneRole role)
    {
        /*                        delete
         *
         *   A ----- R    ->     A       R  =   A       R
         */

        var previousRole = this.state.GetToOneRole(@object, role);
        if (previousRole.IsNull)
        {
            return;
        }

        var inverse = role.Inverse;

        // A <---- R
        this.state.RemoveOneToInverse(previousRole, inverse);

        // A ----> R
        this.state.SetToOneRole(@object, role, default);
    }

    private void SetManyToOneRole(Handle @object, TramManyToOneRole role, Handle normalized)
    {
        /*  [if exist]        [then remove]        set
         *
         *  RA ----- R         RA       R       RA    -- R       RA ----- R
         *                ->                +        -        =       -
         *   A ----- PR         A --x-- PR       A --    PR       A --    PR
         */

        var inverse = role.Inverse;
        var previousRole = this.state.GetToOneRole(@object, role);

        // R = PR
        if (normalized.Equals(previousRole))
        {
            return;
        }

        // A --x-- PR
        if (!previousRole.IsNull)
        {
            this.RemoveFromManyToInverse(previousRole, inverse, @object);
        }

        // A <---- R
        this.AddToManyToInverse(normalized, inverse, @object);

        // A ----> R
        this.state.SetToOneRole(@object, role, normalized);
    }

    private void RemoveManyToOneRole(Handle @object, TramManyToOneRole role)
    {
        /*                        delete
         *  RA --                                RA --
         *       -        ->                 =        -
         *   A ----- R           A --x-- R             -- R
         */

        var previousRole = this.state.GetToOneRole(@object, role);
        if (previousRole.IsNull)
        {
            return;
        }

        var inverse = role.Inverse;

        // A <---- R
        this.RemoveFromManyToInverse(previousRole, inverse, @object);

        // A ----> R
        this.state.SetToOneRole(@object, role, default);
    }

    private void SetOneToManyRole(Handle @object, TramOneToManyRole role, Handles normalizedRole)
    {
        var previousRole = this.state.GetToManyRole(@object, role);

        if (previousRole.IsEmpty)
        {
            if (normalizedRole.IsEmpty)
            {
                return;
            }

            foreach (var normalizedItem in normalizedRole)
            {
                this.AddToOneToManyRole(@object, role, normalizedItem);
            }
        }
        else
        {
            // Use Diff (Add/Remove)
            foreach (var normalizedAddItem in normalizedRole.Except(previousRole))
            {
                this.AddToOneToManyRole(@object, role, normalizedAddItem);
            }

            foreach (var normalizedRemoveItem in previousRole.Except(normalizedRole))
            {
                this.RemoveFromOneToManyRole(@object, role, normalizedRemoveItem);
            }
        }
    }

    private void AddToOneToManyRole(Handle @object, TramOneToManyRole role, Handle normalized)
    {
        /*  [if exist]        [then remove]        add
         *
         *  RA ----- R         RA --x-- R       RA    -- R       RA    -- R
         *                ->                +        -        =       -
         *   A ----- PR         A       PR       A --    PR       A ----- PR
         */

        var previousRole = this.state.GetToManyRole(@object, role);

        // R in PR
        if (previousRole.Contains(normalized))
        {
            return;
        }

        var inverse = role.Inverse;

        // RA --x-- R
        this.TryGet(normalized, inverse, out var roleObject);

        if (!roleObject.IsNull)
        {
            this.RemoveFromOneToManyRole(roleObject, role, normalized);
        }

        // A <---- R
        this.state.SetOneToInverse(normalized, inverse, @object);

        // A ----> R
        var newRole = this.deduplicator.Append(previousRole, normalized);
        this.state.SetToManyRole(@object, role, newRole);
    }

    private void AddToOneToManyRole(Handle @object, TramOneToManyRole role, Handles items)
    {
        var previousRole = this.state.GetToManyRole(@object, role);
        if (previousRole.IsEmpty)
        {
            this.SetOneToManyRole(@object, role, items);
            return;
        }

        var itemsToAdd = this.deduplicator.Except(items, previousRole);

        if (itemsToAdd.Count == 0)
        {
            return;
        }

        var inverse = role.Inverse;

        // Role
        this.state.SetToManyRole(@object, role, this.deduplicator.Concat(previousRole, itemsToAdd));

        // Inverse
        foreach (var item in itemsToAdd)
        {
            this.TryGet(item, inverse, out var previousInverse);

            // One to Many
            if (!previousInverse.IsNull)
            {
                var previousInverseRole = this.state.GetToManyRole(previousInverse, role);
                this.state.SetToManyRole(previousInverse, role, this.deduplicator.Remove(previousInverseRole, item));
            }

            this.state.SetOneToInverse(item, inverse, @object);
        }
    }

    private void RemoveFromOneToManyRole(Handle @object, TramOneToManyRole role, Handle normalized)
    {
        var inverse = role.Inverse;

        var previousRole = this.state.GetToManyRole(@object, role);
        if (!previousRole.Contains(normalized))
        {
            return;
        }

        // Role
        this.state.SetToManyRole(@object, role, this.deduplicator.Remove(previousRole, normalized));

        // Inverse
        this.state.SetOneToInverse(normalized, inverse, default);
    }

    private void RemoveFromOneToManyRole(Handle @object, TramOneToManyRole role, Handles normalized)
    {
        var previousRole = this.state.GetToManyRole(@object, role);
        if (previousRole.IsEmpty)
        {
            return;
        }

        var itemsToRemove = this.deduplicator.Intersect(previousRole, normalized);

        if (itemsToRemove.Count == 0)
        {
            return;
        }

        var inverse = role.Inverse;

        // Role
        this.state.SetToManyRole(@object, role, this.deduplicator.Except(previousRole, itemsToRemove));

        // Inverse
        foreach (var item in itemsToRemove)
        {
            // One to Many
            this.state.RemoveOneToInverse(item, inverse);
        }
    }

    private void RemoveOneToManyRole(Handle @object, TramOneToManyRole role)
    {
        var inverse = role.Inverse;

        // Role
        var previousRole = this.state.GetToManyRole(@object, role);

        this.state.SetToManyRole(@object, role, Handles.Empty);

        // Inverse
        foreach (var previousRoleItem in previousRole)
        {
            this.state.SetOneToInverse(previousRoleItem, inverse, default);
        }
    }

    private void SetManyToManyRole(Handle @object, TramManyToManyRole role, Handles normalizedRole)
    {
        var previousRole = this.state.GetToManyRole(@object, role);

        if (previousRole.IsEmpty)
        {
            if (normalizedRole.IsEmpty)
            {
                return;
            }

            foreach (var normalizedRoleItem in normalizedRole)
            {
                this.AddToManyToManyRole(@object, role, normalizedRoleItem);
            }
        }
        else
        {
            // Use Diff (Add/Remove)
            var toAdd = this.deduplicator.Except(normalizedRole, previousRole);
            var toRemove = this.deduplicator.Except(previousRole, normalizedRole);

            foreach (var addedRole in toAdd)
            {
                this.AddToManyToManyRole(@object, role, addedRole);
            }

            foreach (var removeRole in toRemove)
            {
                this.RemoveFromManyToManyRole(@object, role, removeRole);
            }
        }
    }

    private void AddToManyToManyRole(Handle @object, TramManyToManyRole role, Handle normalized)
    {
        var inverse = role.Inverse;

        var previousRole = this.state.GetToManyRole(@object, role);

        if (previousRole.Contains(normalized))
        {
            return;
        }

        // Role
        var newRole = this.deduplicator.Append(previousRole, normalized);
        this.state.SetToManyRole(@object, role, newRole);

        // Inverse
        var previousInverse = this.state.GetManyToInverse(normalized, inverse);
        this.state.SetManyToInverse(normalized, inverse, this.deduplicator.Append(previousInverse, @object));
    }

    private void AddToManyToManyRole(Handle @object, TramManyToManyRole role, Handles items)
    {
        var inverse = role.Inverse;

        var previousRole = this.state.GetToManyRole(@object, role);
        var itemsToAdd = this.deduplicator.Except(items, previousRole);

        if (itemsToAdd.IsEmpty)
        {
            return;
        }

        // Role
        this.state.SetToManyRole(@object, role, this.deduplicator.Concat(previousRole, itemsToAdd));

        // Inverse
        foreach (var item in itemsToAdd)
        {
            var previousInverse = this.state.GetManyToInverse(item, inverse);
            this.state.SetManyToInverse(item, inverse, this.deduplicator.Append(previousInverse, @object));
        }
    }

    private void RemoveFromManyToManyRole(Handle @object, TramManyToManyRole role, Handle item)
    {
        var inverse = role.Inverse;

        var previousRole = this.state.GetToManyRole(@object, role);
        if (!previousRole.Contains(item))
        {
            return;
        }

        // Role
        this.state.SetToManyRole(@object, role, this.deduplicator.Remove(previousRole, item));

        // Inverse
        var previousInverse = this.state.GetManyToInverse(item, inverse);
        if (previousInverse.Contains(@object))
        {
            this.state.SetManyToInverse(item, inverse, this.deduplicator.Remove(previousInverse, @object));
        }
    }

    private void RemoveManyToManyRole(Handle @object, TramManyToManyRole role)
    {
        var inverse = role.Inverse;

        var previousRole = this.state.GetToManyRole(@object, role);
        if (previousRole.IsEmpty)
        {
            return;
        }

        // Role
        this.state.SetToManyRole(@object, role, Handles.Empty);

        // Inverse
        foreach (var item in previousRole)
        {
            var previousInverse = this.state.GetManyToInverse(item, inverse);
            if (previousInverse.Contains(@object))
            {
                this.state.SetManyToInverse(item, inverse, this.deduplicator.Remove(previousInverse, @object));
            }
        }
    }

    private void RemoveObjectsFromManyToManyRole(Handle @object, TramManyToManyRole role, Handles normalized)
    {
        var previousRole = this.state.GetToManyRole(@object, role);

        if (previousRole.IsEmpty)
        {
            return;
        }

        var itemsToRemove = this.deduplicator.Intersect(previousRole, normalized);

        if (itemsToRemove.IsEmpty)
        {
            return;
        }

        var inverse = role.Inverse;

        // Role
        this.state.SetToManyRole(@object, role, this.deduplicator.Except(previousRole, itemsToRemove));

        // Inverse
        foreach (var item in itemsToRemove)
        {
            var previousInverse = this.state.GetManyToInverse(item, inverse);
            if (previousInverse.Contains(@object))
            {
                this.state.SetManyToInverse(item, inverse, this.deduplicator.Remove(previousInverse, @object));
            }
        }
    }

    private void AddToManyToInverse(Handle role, ITramToManyInverse inverse, Handle @object)
    {
        var previousInverse = this.state.GetManyToInverse(role, inverse);

        if (previousInverse.Contains(@object))
        {
            return;
        }

        this.state.SetManyToInverse(role, inverse, this.deduplicator.Append(previousInverse, @object));
    }

    private void RemoveFromManyToInverse(Handle role, ITramToManyInverse inverse, Handle @object)
    {
        var previousInverse = this.state.GetManyToInverse(role, inverse);

        if (!previousInverse.Contains(@object))
        {
            return;
        }

        this.state.SetManyToInverse(role, inverse, this.deduplicator.Remove(previousInverse, @object));
    }

    private object? Normalize(TramAttribute attribute, object? value)
    {
        return this.normalize?.Invoke(attribute, value) ?? value;
    }

    private Handle Normalize(ITramRelationEnd role, Handle handle)
    {
        if (handle.IsNull)
        {
            return handle;
        }

        if (!this.Exists(handle))
        {
            return default;
        }

        var objectType = this.GetClass(handle);
        if (!role.Type.IsAssignableFrom(objectType))
        {
            throw new ArgumentException(
                $"{role.Name} should be assignable to {role.Type.Name} but was a {objectType.Name}");
        }

        return handle;
    }

    private Handles Normalize(ITramRelationEnd role, IEnumerable<Handle> items)
    {
        var normalized = this.deduplicator.FromUnordered(items.Where(this.Exists).Distinct());

        foreach (var v in normalized)
        {
            var objectType = this.GetClass(v);
            if (!role.Type.IsAssignableFrom(objectType))
            {
                throw new ArgumentException($"{role.Name} should be assignable to {role.Type.Name} but was a {objectType.Name}");
            }
        }

        return normalized;
    }

    private void Assert(Handle handle)
    {
        if (handle.IsNull)
        {
            throw new ArgumentException("Handle is null.", nameof(handle));
        }

        if (!this.Exists(handle))
        {
            throw new ArgumentException($"Object [{handle}] does not exist.", nameof(handle));
        }
    }
}
