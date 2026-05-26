// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Frozen;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Allors.Tram.Schema;

/// <summary>
/// Immutable, read-only snapshot of an <see cref="IReadOnly"/> source. Backed by frozen collections for fast lookups.
/// </summary>
public sealed class Tram : IReadOnly
{
    private readonly FrozenDictionary<Handle, TramClass> classByObject;
    private readonly FrozenDictionary<TramAttribute, FrozenDictionary<Handle, object?>> valueByObjectByAttribute;
    private readonly FrozenDictionary<ITramToOneRelationEnd, FrozenDictionary<Handle, Handle>> toOneByObjectByEnd;
    private readonly FrozenDictionary<ITramToManyRelationEnd, FrozenDictionary<Handle, ImmutableArray<Handle>>> toManyByObjectByEnd;

    /// <summary>
    /// Creates a new frozen snapshot of <paramref name="source"/>.
    /// </summary>
    public Tram(IReadOnly source)
    {
        this.Schema = source.Schema;

        var classes = new Dictionary<Handle, TramClass>();
        var values = new Dictionary<TramAttribute, Dictionary<Handle, object?>>();
        var toOnes = new Dictionary<ITramToOneRelationEnd, Dictionary<Handle, Handle>>();
        var toManys = new Dictionary<ITramToManyRelationEnd, Dictionary<Handle, ImmutableArray<Handle>>>();

        foreach (var @object in source.Objects())
        {
            var @class = source.GetClass(@object);
            classes[@object] = @class;

            foreach (var attribute in @class.Attributes.Cast<TramAttribute>())
            {
                if (source.TryGet(@object, attribute, out var value))
                {
                    if (!values.TryGetValue(attribute, out var byObject))
                    {
                        byObject = new Dictionary<Handle, object?>();
                        values[attribute] = byObject;
                    }

                    byObject[@object] = value;
                }
            }

            foreach (var role in @class.Roles)
            {
                switch (role)
                {
                case ITramToOneRole toOneRole:
                    if (source.TryGet(@object, toOneRole, out var oneRoleItem))
                    {
                        if (!toOnes.TryGetValue(toOneRole, out var byObject))
                        {
                            byObject = new Dictionary<Handle, Handle>();
                            toOnes[toOneRole] = byObject;
                        }

                        byObject[@object] = oneRoleItem;
                    }

                    break;

                case ITramToManyRole toManyRole:
                    if (source.TryGet(@object, toManyRole, out var manyRoleItems))
                    {
                        if (!toManys.TryGetValue(toManyRole, out var byObject))
                        {
                            byObject = new Dictionary<Handle, ImmutableArray<Handle>>();
                            toManys[toManyRole] = byObject;
                        }

                        byObject[@object] = [.. manyRoleItems];
                    }

                    break;
                }
            }

            foreach (var inverse in @class.Inverses)
            {
                switch (inverse)
                {
                case ITramToOneInverse toOneInverse:
                    if (source.TryGet(@object, toOneInverse, out var oneInverseItem))
                    {
                        if (!toOnes.TryGetValue(toOneInverse, out var byObject))
                        {
                            byObject = new Dictionary<Handle, Handle>();
                            toOnes[toOneInverse] = byObject;
                        }

                        byObject[@object] = oneInverseItem;
                    }

                    break;

                case ITramToManyInverse toManyInverse:
                    if (source.TryGet(@object, toManyInverse, out var manyInverseItems))
                    {
                        if (!toManys.TryGetValue(toManyInverse, out var byObject))
                        {
                            byObject = new Dictionary<Handle, ImmutableArray<Handle>>();
                            toManys[toManyInverse] = byObject;
                        }

                        byObject[@object] = [.. manyInverseItems];
                    }

                    break;
                }
            }
        }

        this.classByObject = classes.ToFrozenDictionary();
        this.valueByObjectByAttribute = values.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value.ToFrozenDictionary());
        this.toOneByObjectByEnd = toOnes.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value.ToFrozenDictionary());
        this.toManyByObjectByEnd = toManys.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value.ToFrozenDictionary());
    }

    /// <inheritdoc/>
    public TramSchema Schema { get; }

    /// <inheritdoc/>
    public TramClass GetClass(Handle @object)
    {
        this.Assert(@object);
        return this.classByObject[@object];
    }

    /// <inheritdoc/>
    public IEnumerable<Handle> Objects() => this.classByObject.Keys;

    /// <inheritdoc/>
    public IEnumerable<Handle> ObjectsOfType(TramObjectType objectType) => this.classByObject.Keys.Where(v => objectType.IsAssignableFrom(this.GetClass(v)));

    /// <inheritdoc/>
    public bool Exists(Handle @object) => this.classByObject.ContainsKey(@object);

    /// <inheritdoc/>
    public object? Get(Handle @object, TramAttribute attribute)
    {
        this.Assert(@object);

        if (this.valueByObjectByAttribute.TryGetValue(attribute, out var byObject) &&
            byObject.TryGetValue(@object, out var value))
        {
            return value;
        }

        return null;
    }

    /// <inheritdoc/>
    public Handle Get(Handle @object, ITramToOneRelationEnd role)
    {
        this.Assert(@object);

        if (this.toOneByObjectByEnd.TryGetValue(role, out var byObject) &&
            byObject.TryGetValue(@object, out var item))
        {
            return item;
        }

        return default;
    }

    /// <inheritdoc/>
    public IEnumerable<Handle> Get(Handle @object, ITramToManyRelationEnd role)
    {
        this.Assert(@object);

        if (this.toManyByObjectByEnd.TryGetValue(role, out var byObject) &&
            byObject.TryGetValue(@object, out var items))
        {
            return items;
        }

        return ImmutableArray<Handle>.Empty;
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

    /// <inheritdoc/>
    public bool TryGet(Handle @object, ITramToManyRelationEnd role, out IEnumerable<Handle> items)
    {
        items = this.Get(@object, role);
        return items.Any();
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
